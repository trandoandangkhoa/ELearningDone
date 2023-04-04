namespace WebLearning.Contract.Dtos.Account
{
    public class UpdateAccountDto
    {

        public string Email { get; set; }


        public string Password { get; set; }

        public string PasswordHased { get; set; }

        public int Active { get; set; }

        public Guid RoleId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public AuthorizeRole AuthorizeRole { get; set; }

    }
}
