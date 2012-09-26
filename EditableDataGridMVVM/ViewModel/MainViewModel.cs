using System.Collections.ObjectModel;
using System.Linq;
using EditableDataGridMVVM.Database;
using GalaSoft.MvvmLight;

namespace EditableDataGridMVVM.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		private readonly ApplicationDbContext dbContext;

		public ObservableCollection<ProductViewModel> Products { get; private set; }

		public ObservableCollection<int> Types { get; private set; }

		public MainViewModel(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;

			Types = new ObservableCollection<int>();
			Types.Add(0);
			Types.Add(1);
			Types.Add(2);
			Types.Add(3);

			LoadProducts();
		}

		private void LoadProducts()
		{
			var allProducts = dbContext.Products.ToList().Select(product => new ProductViewModel(product, dbContext));
			Products = new ObservableCollection<ProductViewModel>(allProducts);
		}
	}
}