namespace WebLearning.Contract.Dtos.Assets.Moved
{
    public class AssetsMovedStatusDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AssetsMovedDto> AssetsMovedDtos { get; set; }
    }
}
