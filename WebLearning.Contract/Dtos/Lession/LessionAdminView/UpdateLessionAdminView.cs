using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;

namespace WebLearning.Contract.Dtos.Lession.LessionAdminView
{
    public class UpdateLessionAdminView
    {
        public UpdateLessionDto UpdateLessionDto { get; set; } = new UpdateLessionDto();

        public CreateLessionVideoDto CreateLessionVideoDto { get; set; } = new CreateLessionVideoDto();

        public UpdateLessionVideoDto UpdateLessionVideoDto { get; set; } = new UpdateLessionVideoDto();

        public CreateLessionFileDocumentDto CreateLessionFileDocumentDto { get; set; } = new CreateLessionFileDocumentDto();

    }
}
