using System.ComponentModel.DataAnnotations.Schema;

namespace Livemedy.Core.Base.Entities;

public interface IEntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    DateTime? CreatedDate { get; }
    string CreatedBy { get; }
    DateTime? UpdatedDate { get; }
    string UpdatedBy { get; }
    public bool IsDelete { get; set; }
}
