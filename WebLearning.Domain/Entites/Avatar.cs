using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLearning.Domain.Entites
{
    public class Avatar
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Account")]
        public Guid AccountId { get; set; }

        public string Name { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateCreated { get; set; }

        public long FileSize { get; set; }

        public virtual Account Account { get; set; }
    }
}
