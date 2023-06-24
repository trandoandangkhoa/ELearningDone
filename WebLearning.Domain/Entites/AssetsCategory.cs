using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class AssetsCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string CatCode { get; set; }

        public string Name { get; set; }

        public ICollection<Assests> Assests { get; set; }

    }
}
