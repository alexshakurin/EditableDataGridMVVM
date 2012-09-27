using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EditableDataGridMVVM.Database;
using EditableDataGridMVVM.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ObjectBuilder2;

namespace EditableDataGridMVVM.ViewModel
{
	public class MainViewModel : EditableViewModel
	{
		private readonly ApplicationDbContext dbContext;
		private readonly List<ProductViewModel> deletedProducts;
		private readonly List<ProductViewModel> addedProducts;

		private RelayCommand saveCommand;
		private RelayCommand newItemCommand;
		private RelayCommand cancelCommand;

		public RelayCommand NewItemCommand
		{
			get
			{
				if (newItemCommand == null)
				{
					newItemCommand = new RelayCommand(NewItem);
				}

				return newItemCommand;
			}
		}

		public RelayCommand CancelCommand
		{
			get
			{
				if (cancelCommand == null)
				{
					cancelCommand = new RelayCommand(Cancel);
				}

				return cancelCommand;
			}
		}

		public RelayCommand SaveCommand
		{
			get
			{
				if (saveCommand == null)
				{
					saveCommand = new RelayCommand(Save, CanSave);
				}

				return saveCommand;
			}
		}

		public ObservableCollection<ProductViewModel> Products { get; private set; }

		public ObservableCollection<int> Types { get; private set; }

		public MainViewModel(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
			deletedProducts = new List<ProductViewModel>();
			addedProducts = new List<ProductViewModel>();

			Types = new ObservableCollection<int>();
			Types.Add(0);
			Types.Add(1);
			Types.Add(2);
			Types.Add(3);

			LoadProducts();
		}

		public override void Validate()
		{
			IsValid = true;

			base.Validate();

			if (IsValid)
			{
				Products.ForEach(pr => pr.Validate());
				var hasInvalid = Products.Any(pr => !pr.IsValid);
				if (hasInvalid)
				{
					IsValid = false;
				}
			}
		}

		private void NewItem()
		{
			var product = new Product();
			var viewModel = CreateViewModel(product);
			viewModel.Validate();

			addedProducts.Add(viewModel);

			Products.Add(viewModel);
		}

		private ProductViewModel CreateViewModel(Product product)
		{
			var viewModel = new ProductViewModel(product);
			viewModel.DeleteRequest += OnDeleteRequest;
			viewModel.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == IsValidProperty)
					{
						SaveCommand.RaiseCanExecuteChanged();
					}
				};

			return viewModel;
		}

		private void Cancel()
		{
			foreach (var productViewModel in deletedProducts)
			{
				Products.Add(productViewModel);
			}

			deletedProducts.Clear();

			Products.ForEach(pr => pr.CancelEdit());

			foreach (var productViewModel in addedProducts)
			{
				Products.Remove(productViewModel);
			}

			addedProducts.ForEach(pr => pr.Cleanup());
			addedProducts.Clear();
		}

		private bool CanSave()
		{
			Validate();

			return IsValid;
		}

		private void Save()
		{
			Products.ForEach(pr => pr.EndEdit());

			foreach (var productViewModel in deletedProducts)
			{
				dbContext.Products.Remove(productViewModel.Product);
			}

			foreach (var productViewModel in addedProducts)
			{
				dbContext.Products.Add(productViewModel.Product);
			}

			dbContext.SaveChanges();

			deletedProducts.ForEach(viewModel => viewModel.Cleanup());
			deletedProducts.Clear();
			addedProducts.Clear();
		}

		private void LoadProducts()
		{
			var allProducts = dbContext.Products.ToList().Select(CreateViewModel);

			Products = new ObservableCollection<ProductViewModel>(allProducts);
		}

		private void OnDeleteRequest(object sender, EventArgs e)
		{
			var viewModel = sender as ProductViewModel;
			if (viewModel != null)
			{
				deletedProducts.Add(viewModel);
				viewModel.DeleteRequest -= OnDeleteRequest;

				Products.Remove(viewModel);
			}
		}
	}
}