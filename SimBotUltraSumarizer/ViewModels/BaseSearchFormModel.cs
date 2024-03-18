using SimBotUltraSummarizerDb.Models.Search;
using System.ComponentModel;

namespace SimBotUltraSummarizer.ViewModels
{
    public abstract class BaseSearchFormModel : IBaseSearchFormModel
    {
        [DisplayName("Keywords")]
        public string Keywords { get; set; }
        public string SortBy { get; set; }
        public bool? SortDesc { get; set; }

        public int? Page { get; set; }        
                
        [DisplayName("Page size")]
        public int? RowCount { get; set; } = 15;

        public List<int> RowCounts { get; set; } = new List<int>() { 15, 50, 100, 200, 300, 500 };

        protected virtual void SetSearchRequest(BaseRequest request)
        {
            if (this.Page < 1) { this.Page = null; }            
            if (this.RowCount < 1) { this.RowCount = 15; }

            request.Keywords = this.Keywords;
            request.RowCount = this.RowCount;
            request.Offset = ((this.Page ?? 1) - 1) * this.RowCount;

            if (!string.IsNullOrWhiteSpace(this.SortBy))
            {
                request.SortDesc = this.SortDesc == true;
                request.SortColumn = this.SortBy;
            }
        }

        public virtual void SetDefaultSort(string sortBy, bool? sortDesc = null)
        {
            if (string.IsNullOrEmpty(this.SortBy))
            {
                this.SortBy = sortBy;
                this.SortDesc = sortDesc;
            }
        }
    }
}