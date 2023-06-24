namespace WebLearning.Application.Ultities
{
    public class PagingBase
    {
        public int PageIndex { get; set; } = 1;
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
        public int PageCount
        {
            get
            {
                return TotalRecord;
            }
        }

    }
}
