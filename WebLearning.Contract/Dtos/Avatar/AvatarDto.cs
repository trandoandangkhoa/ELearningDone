using WebLearning.Contract.Dtos.Account;

namespace WebLearning.Contract.Dtos.Avatar
{
    public class AvatarDto
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public long FileSize { get; set; }

        public virtual AccountDto AccountDto { get; set; }
    }
}
