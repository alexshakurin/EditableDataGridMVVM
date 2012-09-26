using System.Data.Entity;
using EditableDataGridMVVM.Model;

namespace EditableDataGridMVVM.Database
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }

		public ApplicationDbContext(string connectionString) : base(connectionString)
		{
		}
	}
}