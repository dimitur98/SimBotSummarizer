using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class SignalDatas
    {
        public static DateTime GetLastDate()
        {
            var sql = "SELECT `date` FROM `signal_data` ORDER BY `date` DESC LIMIT 1";

            return Db.Mapper.Query<DateTime>(sql).FirstOrDefault();
        }

        public static List<SignalData> GetBySignalIds(IEnumerable<uint> signalIds)
        {
            var sql = @"SELECT *
                FROM signal_data
                WHERE signal_id IN @signalIds
                ORDER BY `date`";

            return Db.Mapper.Query<SignalData>(sql, new { signalIds }).ToList();
        }

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
                    `signal_id`,
					`address`,
                    `mcap`,
                    `total_calls`,
                    `sell_tax`,
                    `buy_tax`,
                    `price`,
                    `date`
				) VALUES (
                    @signalId,
					@address,
                    @mcap,
                    @totalCalls,
                    @sellTax,
                    @buyTax,
                    @price,
                    @date
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                signalId = signalData.SignalId,
                address = signalData.Address,
                mcap = signalData.MCap,
                totalCalls = signalData.TotalCalls,
                sellTax = signalData.SellTax,
                buyTax = signalData.BuyTax,
                price = signalData.Price,
                date = signalData.Date
            };

            signalData.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }
    }
}
