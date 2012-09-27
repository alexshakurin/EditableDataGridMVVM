using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using AutoMapper;
using EditableDataGridMVVM.Attributes;
using EditableDataGridMVVM.Model;
using GalaSoft.MvvmLight.Command;

namespace EditableDataGridMVVM.ViewModel
{
	public class ProductViewModel : EditableViewModel
	{
		public event EventHandler DeleteRequest;

		public Product Product { get; private set; }

		private string code;
		private int type;
		private int unit;

		private ICommand deleteCommand;

		public ICommand DeleteCommand
		{
			get
			{
				if (deleteCommand == null)
				{
					deleteCommand = new RelayCommand(Delete);
				}

				return deleteCommand;
			}
		}

		[CacheProperty]
		[Required]
		[MaxLength(30)]
		public string Code
		{
			get
			{
				return code;
			}
			set
			{
				if (code == value)
				{
					return;
				}

				code = value;
				RaisePropertyChanged(() => Code);
			}
		}

		[CacheProperty]
		public int Type
		{
			get
			{
				return type;
			}
			set
			{
				if (type == value)
				{
					return;
				}

				type = value;
				RaisePropertyChanged(() => Type);
			}
		}

		[CacheProperty]
		public int Unit
		{
			get
			{
				return unit;
			}
			set
			{
				if (unit == value)
				{
					return;
				}

				unit = value;
				RaisePropertyChanged(() => Unit);
			}
		}

		public ProductViewModel(Product product)
		{
			Product = product;

			LoadProduct();
		}

		public override void EndEdit()
		{
			Validate();
			if (IsValid)
			{
				UpdateProduct();
			}
		}

		public override void Validate()
		{
			base.Validate();
			// TODO: add custom validate logic here
		}

		private void Delete()
		{
			OnDeleteRequest(EventArgs.Empty);
		}

		private void OnDeleteRequest(EventArgs args)
		{
			var local = DeleteRequest;
			if (local != null)
			{
				local(this, args);
			}
		}

		private void LoadProduct()
		{
			Mapper.Map(Product, this);
		}

		private void UpdateProduct()
		{
			Mapper.DynamicMap<ProductViewModel, Product>(this, Product);
		}
	}
}