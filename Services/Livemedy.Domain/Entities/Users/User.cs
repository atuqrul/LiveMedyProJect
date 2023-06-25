using Livemedy.Core.Base.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Livemedy.Domain.Entities.Users
{
    [Table("Users", Schema = "dbo")]
    public class User : EntityBase
    {

        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        
        [Required]
        //[HashProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public int RoleId { get; set; }
    }
}
