using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHased { get; set; }
        public int Active { get; set; }
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLogin { get; set; }
        public virtual AccountDetail AccountDetail { get; set; }
        public virtual Role Role { get; set; }
        public AuthorizeRoles AuthorizeRole { get; set; }
        public Avatar Avatar { get; set; }
       

    }
}
