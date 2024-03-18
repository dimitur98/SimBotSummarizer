namespace SimBotUltraSummarizer.ViewModels
{
    public class Pager : BasePager
    {
        public IEnumerable<int> PageSizes { get; set; }
        public string CssClass { get; set; }
        public bool ShowCreateButton { get; set; } = true;
        public object CreateButtonRouteValues { get; set; }


        public Pager(long page, int pageSize, long totalItemsCount, IEnumerable<int> pageSizes = null, string cssClass = null, bool showCreateButton = true, object createButtonRouteValues = null)
            : base(page, pageSize, totalItemsCount)
        {
            this.PageSizes = pageSizes;
            this.CssClass = cssClass;
            this.ShowCreateButton = showCreateButton;
            this.CreateButtonRouteValues = createButtonRouteValues;

            this.MaxDisplayedPages = 8;
        }
    }
}