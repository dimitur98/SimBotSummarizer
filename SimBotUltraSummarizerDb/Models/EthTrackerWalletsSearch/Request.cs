using SimBotUltraSummarizerDb.Models.Search;

namespace SimBotUltraSummarizerDb.Models.EthTrackerWalletsSearch
{
    public class Request : BaseRequest
    {
        public uint? Id { get; set; }
        public uint? EthTrackerTypeId { get; set; }
    }
}
