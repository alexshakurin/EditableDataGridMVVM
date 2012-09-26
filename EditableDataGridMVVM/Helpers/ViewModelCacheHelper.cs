using System;
using System.Collections.Generic;
using System.Reflection;
using EditableDataGridMVVM.Attributes;
using EditableDataGridMVVM.ViewModel;
using GalaSoft.MvvmLight;

namespace EditableDataGridMVVM.Helpers
{
	public class ViewModelCacheHelper : ICleanup
	{
		private readonly List<PropertyInfo> properties;
		private readonly Dictionary<string, object> cache;

		private EditableViewModel viewModel;

		public ViewModelCacheHelper(EditableViewModel viewModel)
		{
			if (viewModel == null)
			{
				throw new ArgumentNullException("viewModel");
			}

			properties = new List<PropertyInfo>();
			cache = new Dictionary<string, object>();
			this.viewModel = viewModel;

			GetCacheProperties();
		}

		public void Cleanup()
		{
			viewModel = null;
		}

		public void StoreToCache()
		{
			foreach (var property in properties)
			{
				cache[property.Name] = property.GetValue(viewModel, null);
			}
		}

		public void RestoreFromCache()
		{
			foreach (var property in properties)
			{
				object cachedValue;
				if (cache.TryGetValue(property.Name, out cachedValue))
				{
					property.SetValue(viewModel, cachedValue, null);
				}
			}
		}

		private void GetCacheProperties()
		{
			var typeProperties = viewModel.GetType().GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

			foreach (var property in typeProperties)
			{
				var attributes = property.GetType().GetCustomAttributes(typeof (CachePropertyAttribute), true);
				if (attributes.Length > 0)
				{
					properties.Add(property);
				}
			}
		}
	}
}