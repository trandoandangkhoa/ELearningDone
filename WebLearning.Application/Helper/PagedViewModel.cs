using WebLearning.Contract.Dtos.Assets;

namespace WebLearning.Application.Ultities
{
    public class PagedViewModel<T> : PagingBase
    {
        public IEnumerable<T> Items { get; set; }
        public CheckBox CheckBox { get; set; }
        public CheckBox HistoryChecked { get;set; }
        
    }
}
