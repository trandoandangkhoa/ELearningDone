using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class OtherFileUpload
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Lession")]
        public Guid LessionId { get; set; }

        public string Caption { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public int SortOrder { get; set; }

        public long FileSize { get; set; }

        public virtual Lession Lession { get; set; }

    }
}
