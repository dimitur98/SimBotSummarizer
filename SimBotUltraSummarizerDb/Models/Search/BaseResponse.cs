using SimBotUltraSummarizerDb.Contracts;

namespace SimBotUltraSummarizerDb.Models.Search
{
    public abstract class BaseResponse<T> : IBaseResponse<T>
    {
        public IEnumerable<T> Records { get; set; }
        public long TotalRecords { get; set; }

        protected BaseResponse()
        {
            this.Records = new List<T>();
        }
    }
}
