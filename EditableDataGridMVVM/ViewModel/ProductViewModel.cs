using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EditableDataGridMVVM.Attributes;
using EditableDataGridMVVM.Database;
using EditableDataGridMVVM.Model;

namespace EditableDataGridMVVM.ViewModel
{
	public class ProductViewModel : EditableViewModel
	{
		private readonly Product product;
		private readonly ApplicationDbContext dbContext;

		private string code;
		private int type;
		private int unit;

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

		public ProductViewModel(Product product, ApplicationDbContext dbContext)
		{
			this.product = product;
			this.dbContext = dbContext;

			LoadProduct();
		}

		public override void EndEdit()
		{
			Validate();
			if (IsValid)
			{
				UpdateProduct();
				dbContext.SaveChanges();
			}
		}

		public override void Validate()
		{
			base.Validate();
			// TODO: add custom validate logic here
		}

		private void LoadProduct()
		{
			Mapper.Map(product, this);
		}

		private void UpdateProduct()
		{
			Mapper.DynamicMap<ProductViewModel, Product>(this, product);
		}
	}
}