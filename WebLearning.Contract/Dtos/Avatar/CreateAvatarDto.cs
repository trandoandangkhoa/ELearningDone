using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.Avatar
{
    public class CreateAvatarDto
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public long FileSize { get; set; }

        public IFormFile Image { get; set; }


    }
}
