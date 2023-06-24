namespace WebLearning.Contract.Dtos.Assets.Category
{
    public class AssetsCategoryDto
    {
        public Guid Id { get; set; }

        public string CatCode { get; set; }

        public string Name { get; set; }


        public IEnumerable<AssetsDto> AssetsDtos { get; set; }
    }
}
