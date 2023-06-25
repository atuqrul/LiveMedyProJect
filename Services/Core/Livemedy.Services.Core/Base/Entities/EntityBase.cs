using System.ComponentModel.DataAnnotations.Schema;

namespace Livemedy.Core.Base.Entities;

public abstract class EntityBase : IEntityBase
{
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(Order = 60)]
    public DateTime? CreatedDate { get; set; }
    [Column(Order = 65)]
    public string CreatedBy { get; set; }
    [Column(Order = 70)]
    public DateTime? UpdatedDate { get; set; }
    [Column(Order = 75)]
    public string UpdatedBy { get; set; }
    [Column(Order = 50)]
    public bool IsDelete { get; set; }

    public EntityBase Clone()
    {
        return (EntityBase)this.MemberwiseClone();
    }
}  
