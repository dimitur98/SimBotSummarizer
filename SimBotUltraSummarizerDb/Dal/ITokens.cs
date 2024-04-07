using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class ITokens
    {
        public static DateTime GetLastDate()
        {
            var sql = "SELECT `date` FROM `itoken` ORDER BY `date` DESC LIMIT 1";

            return Db.Mapper.Query<DateTime>(sql).FirstOrDefault();
        }

        public static List<IToken> GetByAddresses(IEnumerable<string> addresses)
        {
            var sql = @"SELECT *
                FROM itoken
                WHERE address IN @addresses";

            return Db.Mapper.Query<IToken>(sql, new { addresses }).ToList();
        }

        public static void Insert(IToken itoken)
        {
            string sql = @"
				INSERT INTO `itoken` (
					`address`,
                    `scan`,
                    `date`
				) VALUES (
					@address,
                    @scan,
                    @date
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                address = itoken.Address.ToLower().Trim(),
                scan = itoken.Scan,
                date = itoken.Date
            };

            itoken.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }
    }
}
