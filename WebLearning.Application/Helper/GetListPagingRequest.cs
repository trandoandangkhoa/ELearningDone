namespace WebLearning.Application.Ultities
{
    public class GetListPagingRequest : PagingBase
    {
        public string Keyword { get; set; }

        public string AccountName { get; set; }

        public Guid RoleId { get; set; }

    }
}
