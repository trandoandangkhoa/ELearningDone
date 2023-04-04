namespace WebLearning.Contract.Dtos.Account
{
    public class AccountDetailDto
    {
        public Guid Id { get; set; }
        public Guid AccoundId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

    }
}
