using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos.LessionFileDocument
{
    internal class UpdateLessionFileDocumentDto
    {

        public string Caption { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public IFormFile FileDocument { get; set; }

    }
}
