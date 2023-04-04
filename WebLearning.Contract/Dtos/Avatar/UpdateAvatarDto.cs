using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.Avatar
{
    public class UpdateAvatarDto
    {
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public long FileSize { get; set; }

        public IFormFile Image { get; set; }


    }
}
