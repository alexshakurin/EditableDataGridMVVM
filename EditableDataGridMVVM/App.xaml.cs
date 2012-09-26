using System.Configuration;
using System.IO;
using System.Windows;
using AutoMapper;
using EditableDataGridMVVM.Database;
using EditableDataGridMVVM.Helpers;
using EditableDataGridMVVM.Model;
using EditableDataGridMVVM.ViewModel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace EditableDataGridMVVM
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public App()
		{
			var path = Path.GetDirectoryName(GetType().Assembly.Location).Trim('\\');

			var connectionString = ConfigurationManager.ConnectionStrings["ApplicationDb"].ConnectionString
				.Replace("|DataDirectory|", Path.Combine(path, "App_Data"));

			var container = new UnityContainer();
			container.RegisterType<ApplicationDbContext>(
				new InjectionConstructor(connectionString));

			container.RegisterType<MainViewModel>();

			var locator = new UnityServiceLocator(container);

			ServiceLocator.SetLocatorProvider(() => locator);

			Mapper.CreateMap<Product, ProductViewModel>()
				.ForMember(ex => ex.Error, opt => opt.Ignore())
				.ForMember(ex => ex.IsValid, opt => opt.Ignore())
				.ForMember(ex => ex.IsInDesignMode, opt => opt.Ignore());

			Mapper.CreateMap<ProductViewModel, Product>()
				.ForMember(pr => pr.ProductId, opt => opt.Ignore());
		}
	}
}