using Livemedy.Core.Base.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Livemedy.Domain.Entities.Users;

[Table("Roles", Schema = "dbo")]
public class Role : EntityBase
{
    public string Name { get; set; }
}
