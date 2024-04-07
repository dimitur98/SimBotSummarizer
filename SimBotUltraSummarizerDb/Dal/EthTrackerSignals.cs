using SimBotUltraSummarizerDb.Models;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class EthTrackerSignals
    {
        public static List<EthTrackerSignal> GetByAddresses(IEnumerable<string> addresses)
        {
            var sql = "SELECT * FROM eth_tracker_signal WHERE address IN @addresses";

            return Db.Mapper.Query<EthTrackerSignal>(sql, new { addresses }).ToList();
        }

        public static Dictionary<uint, int> GetWalletSignalsCount(IEnumerable<string> walletAddresses)
        {
            var sql = @"SELECT ets.eth_tracker_wallet_id AS EthTrackerWalletId, COUNT(ets.address) AS Count
                FROM eth_tracker_signal ets
                WHERE EXISTS(SELECT * FROM eth_tracker_wallet etw WHERE ets.eth_tracker_wallet_id = etw.id AND etw.address IN @walletAddresses)
                    AND EXISTS(SELECT * FROM `signal` s WHERE s.address = ets.address)
                GROUP BY ets.eth_tracker_wallet_id";

            return Db.Mapper.Query(sql, param: new { walletAddresses }).ToDictionary(x => (uint)x.EthTrackerWalletId, x => (int)x.Count);
        }

        public static DateTime GetLastDate(uint ethTrackerTypeId)
        {
            var sql = @"SELECT `date`
                FROM `eth_tracker_signal` ets
                WHERE EXISTS(SELECT * FROM eth_tracker_wallet etw WHERE etw.id = ets.eth_tracker_wallet_id AND etw.eth_tracker_type_id = @ethTrackerTypeId) 
                ORDER BY `date` DESC 
                LIMIT 1";

            return Db.Mapper.Query<DateTime>(sql, param: new { ethTrackerTypeId }).FirstOrDefault();
        }

        public static void Insert(EthTrackerSignal ethTrackerSignal)
        {
            string sql = @"
				INSERT INTO `eth_tracker_signal` (
                    `eth_tracker_wallet_id`,
					`address`,
                    `date`
				) VALUES (
                    @ethTrackerWalletId,
					@address,
                    @date
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                ethTrackerWalletId = ethTrackerSignal.EthTrackerWalletId,
                address = ethTrackerSignal.Address,
                date = ethTrackerSignal.Date
            };

            ethTrackerSignal.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }

        public static void LoadWallets(IEnumerable<EthTrackerSignal> signals)
        {
            Db.LoadEntities(signals, x => x.EthTrackerWalletId, walletIds => EthTrackerWallets.GetByIds(walletIds), (signal, wallets) => signal.EthTrackerWallet = wallets.FirstOrDefault(x => x.Id == signal.EthTrackerWalletId));
        }
    }
}
