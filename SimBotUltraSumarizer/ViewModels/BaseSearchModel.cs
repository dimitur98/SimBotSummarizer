using SimBotUltraSummarizerDb.Models.Search;

namespace SimBotUltraSummarizer.ViewModels
{
    public abstract class BaseSearchModel<TSearchForm, TSearchResponse, T>
        where TSearchForm : IBaseSearchFormModel
        where TSearchResponse : BaseResponse<T>
        where T : class
    {
        public TSearchForm SearchForm { get; set; }
        public TSearchResponse Response { get; set; }

        protected BaseSearchModel() {}

        protected BaseSearchModel(TSearchForm searchForm)
        {
            this.SearchForm = searchForm;
        }
        public virtual Pager ToPager(string cssClass = null, bool showCreateButton = true, object createButtonRouteValues = null)
        {
            return new Pager(this.SearchForm.Page ?? 1, this.SearchForm.RowCount ?? 15, this.Response.TotalRecords, pageSizes: this.SearchForm.RowCounts, cssClass: cssClass, showCreateButton: showCreateButton, createButtonRouteValues: createButtonRouteValues);
        }
    }
}