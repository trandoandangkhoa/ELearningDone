using System.ComponentModel.DataAnnotations;

namespace WebLearning.Domain.Entites
{
    public class AssetCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string CatCode { get; set; }

        public string Name { get; set; }

        public ICollection<Asset> Assests { get; set; }

    }
}
