using Microsoft.AspNetCore.Http;

namespace WebLearning.Contract.Dtos
{
    public class ImportResponse
    {
        public IFormFile File { get; set; }
        public string Msg { get; set; }


    }
}
