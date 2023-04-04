namespace WebLearning.Application.Ultities
{
    public class PagingBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public int TotalRecord { get; set; }
        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecord / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }

    }
}
