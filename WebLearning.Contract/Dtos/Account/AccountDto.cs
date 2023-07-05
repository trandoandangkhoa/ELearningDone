using WebLearning.Contract.Dtos.Avatar;
using WebLearning.Contract.Dtos.Role;

namespace WebLearning.Contract.Dtos.Account
{
    public class AccountDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }

        public string PasswordHased { get; set; }

        public int Active { get; set; }

        public Guid RoleId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public AccountDetailDto accountDetailDto { get; set; }

        public RoleDto roleDto { get; set; }

        public AuthorizeRole AuthorizeRole { get; set; }

        public AvatarDto Avatar { get; set; }



    }
}
