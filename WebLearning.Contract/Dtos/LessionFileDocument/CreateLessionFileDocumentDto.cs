using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.LessionFileDocument
{
    public class CreateLessionFileDocumentDto
    {
        public Guid Id { get; set; }

        public Guid LessionId { get; set; }

        public string Caption { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public IFormFile FileDocument { get; set; }

    }
}
