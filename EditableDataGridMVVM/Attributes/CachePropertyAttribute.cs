using System;

namespace EditableDataGridMVVM.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class CachePropertyAttribute : Attribute
	{
	}
}