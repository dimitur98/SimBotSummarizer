namespace SimBotUltraSummarizerDb.Models.Search
{
    public abstract class BaseRequest
    {
        public string Keywords { get; set; }
        public bool? IsActive { get; set; }

        public string SortColumn { get; set; }
        public bool SortDesc { get; set; }

        public int? Offset { get; set; }
        public int? RowCount { get; set; }

        public bool ReturnTotalRecords { get; set; } = true;

        public bool ReturnRecords { get; set; } = true;
    }
}
