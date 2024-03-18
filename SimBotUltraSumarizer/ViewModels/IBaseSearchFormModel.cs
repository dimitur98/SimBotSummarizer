namespace SimBotUltraSummarizer.ViewModels
{
    public interface IBaseSearchFormModel
    {
        string Keywords { get; set; }
        string SortBy { get; set; }
        bool? SortDesc { get; set; }
        int? Page { get; set; }        
        int? RowCount { get; set; }
        List<int> RowCounts { get; }
    }
}