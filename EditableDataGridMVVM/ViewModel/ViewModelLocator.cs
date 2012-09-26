using Microsoft.Practices.ServiceLocation;

namespace EditableDataGridMVVM.ViewModel
{
	public class ViewModelLocator
	{
		private MainViewModel main;

		public MainViewModel Main
		{
			get
			{
				if (main == null)
				{
					main = ServiceLocator.Current.GetInstance<MainViewModel>();
				}

				return main;
			}
		}
	}
}