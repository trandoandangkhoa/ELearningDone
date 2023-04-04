namespace WebLearning.Contract.Dtos.Account
{
    public class CreateAccountDto
    {
        public Guid Id = Guid.NewGuid();

        public string Code { get; set; }

        public string CodeRole { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHased { get; set; }

        public int Active { get; set; }

        public Guid RoleId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastLogin { get; set; }

        public Guid AccountDetailId = Guid.NewGuid();

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public AuthorizeRole AuthorizeRole { get; set; }


    }
}
