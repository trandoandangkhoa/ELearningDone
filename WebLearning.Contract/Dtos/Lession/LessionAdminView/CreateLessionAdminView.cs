using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.Contract.Dtos.Lession.LessionAdminView
{
    public class CreateLessionAdminView
    {
        public CreateLessionDto CreateLessionDto { get; set; } = new CreateLessionDto();

        public CreateLessionVideoDto CreateLessionVideoDto { get; set; }

        public CreateLessionFileDocumentDto CreateLessionFileDocumentDto { get; set; }
    }
}
