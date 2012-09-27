using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EditableDataGridMVVM.Helpers;
using GalaSoft.MvvmLight;

namespace EditableDataGridMVVM.ViewModel
{
	public class EditableViewModel : ViewModelBase, IDataErrorInfo
	{
		protected const string IsValidProperty = "IsValid";

		private readonly Dictionary<string, string> errors;
		private readonly ViewModelCacheHelper cacheHelper;

		private string error;
		private bool isValid;

		public string this[string columnName]
		{
			get
			{
				string columnError;
				errors.TryGetValue(columnName, out columnError);

				return columnError;
			}
		}

		public string Error
		{
			get
			{
				return error;
			}
			set
			{
				if (error == value)
				{
					return;
				}

				error = value;
				RaisePropertyChanged(() => Error);
			}
		}

		public bool IsValid
		{
			get
			{
				return isValid;
			}
			set
			{
				if (isValid == value)
				{
					return;
				}

				isValid = value;
				RaisePropertyChanged(() => IsValid);
			}
		}

		public EditableViewModel()
		{
			errors = new Dictionary<string, string>();
			cacheHelper = new ViewModelCacheHelper(this);
		}

		public virtual void BeginEdit()
		{
			StoreToCache();
		}

		public virtual void EndEdit()
		{
			Validate();
		}

		public virtual void CancelEdit()
		{
			RestoreFromCache();
		}

		public override void Cleanup()
		{
			base.Cleanup();
			cacheHelper.Cleanup();
		}

		public virtual void Validate()
		{
			var context = new ValidationContext(this, null, null);
			var validationErrors = new List<ValidationResult>();
			var result = Validator.TryValidateObject(this, context, validationErrors);

			Error = null;
			IsValid = result;

			ClearErrors();
			if (!result)
			{
				foreach (var validationResult in validationErrors)
				{
					SetError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
				}
			}
		}

		private void SetError(string propertyName, string message)
		{
			errors[propertyName] = message;
			Error = message;
			IsValid = false;
		}

		private void ClearErrors()
		{
			var keys = errors.Keys.ToList();
			foreach (var key in keys)
			{
				errors[key] = string.Empty;
			}
		}

		private void StoreToCache()
		{
			cacheHelper.StoreToCache();
		}

		private void RestoreFromCache()
		{
			cacheHelper.RestoreFromCache();
		}
	}
}