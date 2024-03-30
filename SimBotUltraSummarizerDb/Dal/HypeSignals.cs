using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class HypeSignals
    {
        public static DateTime GetLastDate()
        {
            var sql = "SELECT `date` FROM `hype_signal` ORDER BY `date` DESC LIMIT 1";

            return Db.Mapper.Query<DateTime>(sql).FirstOrDefault();
        }

        public static List<HypeSignal> GetByAddress(IEnumerable<string> addresses)
        {
            var sql = @"SELECT *
                FROM hype_signal
                WHERE address IN @addresses";

            return Db.Mapper.Query<HypeSignal>(sql, new { addresses }).ToList();
        }

        public static void Insert(HypeSignal hypeSignal)
        {
            string sql = @"
				INSERT INTO `hype_signal` (
					`address`,
                    `mcap`,
                    `alarm_type`,
                    `date`
				) VALUES (
					@address,
                    @mcap,
                    @alarmType,
                    @date
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                address = hypeSignal.Address,
                mcap = hypeSignal.MCap,
                alarmType = hypeSignal.AlarmType.Trim(),
                date = hypeSignal.Date
            };

            hypeSignal.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }
    }
}
