namespace WebLearning.Application.Ultities
{
    public class GetListPagingRequest : PagingBase
    {
        public string Keyword { get; set; }

        public Guid[] AssetsCategoryId { get; set; }
        public Guid[] AssetsDepartmentId { get; set; }

        public string[] AssetsDepartmentLocationId { get; set; }

        public int[] AssetsStatusId { get; set; }
        public bool Active { get; set; }

        public string AccountName { get; set; }

        public Guid RoleId { get; set; }

    }
}
