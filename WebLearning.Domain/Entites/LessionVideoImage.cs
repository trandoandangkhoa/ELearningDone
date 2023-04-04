namespace WebLearning.Domain.Entites
{
    public class LessionVideoImage
    {
        public Guid Id { get; set; }

        public Guid LessionId { get; set; }

        public string Code { get; set; }


        public string Caption { get; set; }

        public string ImagePath { get; set; }

        public string LinkVideo { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public string Alias { get; set; }


        public bool Notify { get; set; }

        public string DescNotify { get; set; }

        public virtual Lession Lession { get; set; }

    }
}
