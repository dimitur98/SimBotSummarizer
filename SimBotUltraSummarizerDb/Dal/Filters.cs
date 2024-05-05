using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class Filters
    {
        public static List<Filter> GetAll()
        {
            var sql = @"SELECT *
                FROM `filter`";

            return Db.Mapper.Query<Filter>(sql).ToList();
        }
    }
}
