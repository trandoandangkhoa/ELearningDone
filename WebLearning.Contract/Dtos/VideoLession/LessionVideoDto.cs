using WebLearning.Contract.Dtos.Lession;

namespace WebLearning.Contract.Dtos.VideoLession
{
    public class LessionVideoDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Guid LessionId { get; set; }
        public string Caption { get; set; }
        public string ImagePath { get; set; }

        public string LinkVideo { get; set; }

        public DateTime DateCreated { get; set; }
        public int SortOrder { get; set; }
        public long FileSize { get; set; }
        public string Alias { get; set; }
        public bool Notify { get; set; }

        public string DescNotify { get; set; }
        public LessionDto LessionDto { get; set; }
    }
}
