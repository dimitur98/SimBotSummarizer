using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class ITokens
    {
        public static List<IToken> GetByAddress(IEnumerable<string> addresses)
        {
            var sql = @"SELECT *
                FROM itoken
                WHERE address IN @addresses";
            var a =  Db.Mapper.Query<IToken>(sql, new { addresses }).ToList();
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
