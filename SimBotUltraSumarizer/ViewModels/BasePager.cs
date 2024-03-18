namespace SimBotUltraSummarizer.ViewModels
{
    public class BasePager
    {
        public long TotalItemsCount { get; set; }
        public long Page { get; set; }
        public int PageSize { get; set; } = 10;
        public int MaxDisplayedPages { get; set; } = 10;

        public long PagesCount
        {
            get
            {
                var result = (long)Math.Ceiling(this.TotalItemsCount / (decimal)this.PageSize);

                return (result < 1) ? 1 : result;
            }
        }

        public IEnumerable<long> DisplayedPages
        {
            get
            {
                var page = this.Page;
                var pagesCount = this.PagesCount;
                var result = new List<long>();
                var start = Math.Max(page - (long)Math.Floor(this.MaxDisplayedPages / (decimal)2), 1);
                var end = Math.Min((start - 1) + this.MaxDisplayedPages, pagesCount);

                for (var i = start; i <= end; i++)
                {
                    result.Add(i);
                }

                return result;
            }
        }

        public BasePager(long page, int pageSize, long totalItemsCount)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalItemsCount = totalItemsCount;
        }
    }
}
