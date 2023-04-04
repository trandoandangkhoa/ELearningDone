using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.VideoLession
{
    public class UpdateLessionVideoDto
    {

        public string Caption { get; set; }
        public string Code { get; }

        public string ImagePath { get; set; }

        public string LinkVideo { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public IFormFile ImageFile { get; set; }

        public string Alias { get; set; }

        public bool Notify { get; set; }

        public string DescNotify { get; set; }

    }
}
