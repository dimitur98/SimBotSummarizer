using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class EthTrackerTypes
    {
        public const uint MainAccount = 1;
        public const uint SecondAccount = 2;

        public static List<EthTrackerType> GetAll()
        {
            var sql = "SELECT * FROM eth_tracker_type";

            return Db.Mapper.Query<EthTrackerType>(sql).ToList();
        }
    }
}
