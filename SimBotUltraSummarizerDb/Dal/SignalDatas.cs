using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class SignalDatas
    {
        public static List<SignalData> GetByAddress(IEnumerable<string> addresses)
        {
            var sql = @"SELECT *
                FROM signal_data
                WHERE address IN @addresses
                ORDER BY `date`";

            return Db.Mapper.Query<SignalData>(sql, new { addresses }).ToList();
        }

        public static void Insert(SignalData signalData)
        {
            string sql = @"
				INSERT INTO `signal_data` (
					`address`,
                    `mcap`,
                    `total_calls`,
                    `sell_tax`,
                    `buy_tax`,
                    `date`
				) VALUES (
					@address,
                    @mcap,
                    @totalCalls,
                    @sellTax,
                    @buyTax,
                    @date
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                address = signalData.Address,
                mcap = signalData.MCap,
                totalCalls = signalData.TotalCalls,
                sellTax = signalData.SellTax,
                buyTax = signalData.BuyTax,
                date = signalData.Date
            };

            signalData.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }
    }
}
