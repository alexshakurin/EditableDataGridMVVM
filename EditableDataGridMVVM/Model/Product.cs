using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EditableDataGridMVVM.Model
{
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int ProductId { get; set; }

		[Required]
		[MaxLength(30)]
		public virtual string Code { get; set; }

		public virtual int Type { get; set; }

		public virtual int Unit { get; set; }
	}
}