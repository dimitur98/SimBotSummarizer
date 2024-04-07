using SimBotUltraSummarizerDb.Models;
using SimBotUltraSummarizerDb.Models.EthTrackerWalletsSearch;
using SqlQueryBuilder.MySql;
using System.Data;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class EthTrackerWallets
    {
        public static Response Search(Request request)
        {
            var query = new Query
            {
                Select = new List<string>() { "etw.*" },
                From = "`eth_tracker_wallet` AS etw",
                Where = new List<string>() { "1 = 1" },
                Joins = new List<string>()
            };

            //set default sorting
            if (string.IsNullOrEmpty(request.SortColumn))
            {
                request.SortColumn = "name";
                request.SortDesc = false;
            }

            query.OrderBys = new List<OrderBy>() { new OrderBy(request.SortColumn, request.SortDesc) };

            if (!string.IsNullOrEmpty(request.Keywords)) { query.Where.Add("AND (etw.name LIKE @keywords)"); }
            if (request.Id.HasValue) { query.Where.Add(" AND  etw.id = @id"); }
            if (request.EthTrackerTypeId.HasValue) { query.Where.Add(" AND  etw.eth_tracker_type_id = @ethTrackerTypeId"); }
           

            query.Limit = new Limit(request.Offset, request.RowCount);

            var response = new Response();

            using (var connection = Db.Mapper.GetConnection())
            {
                var queryParams = new
                {
                    keywords = string.Format("%{0}%", request.Keywords),
                    id = request.Id,
                    ethTrackerTypeId = request.EthTrackerTypeId,
                };

                //get TotalRecordsCount
                if (request.ReturnTotalRecords)
                {
                    response.TotalRecords = Db.QueryCount(connection, query, param: queryParams);

                    if (response.TotalRecords <= 0) { return response; }
                }

                response.Records = Db.Mapper.Query<EthTrackerWallet>(connection, query.Build(), queryParams, commandType: CommandType.Text);
            }

            return response;
        }

        public static List<EthTrackerWallet> GetAll(bool? isActive = true, bool loadType = true)
        {
            var query = new Query
            {
                Select = new List<string> { "*" },
                From = "eth_tracker_wallet etw",
                Where = new List<string> { "1=1" },
            };

            if (isActive.HasValue) { query.Where.Add(" AND etw.is_active = @isActive"); }

            var response = Db.Mapper.Query<EthTrackerWallet>(query.ToString(), new { isActive }).ToList();

            if (loadType) { LoadTypes(response); }

            return response;
        }

        public static EthTrackerWallet Get(string address, string name, uint ethTrackerTypeId) 
        {
            var sql = @"SELECT *
                FROM eth_tracker_wallet
                WHERE address = @address
                    AND eth_tracker_type_id = @ethTrackerTypeId";

            if (!string.IsNullOrEmpty(name)) { sql += " AND `name` = @name"; }

            return Db.Mapper.Query<EthTrackerWallet>(sql, new { address, name, ethTrackerTypeId }).FirstOrDefault();
        }

        public static List<EthTrackerWallet> GetByIds(IEnumerable<uint> ids)
        {
            var sql = @"SELECT *
                FROM eth_tracker_wallet
                WHERE id IN @ids";

            return Db.Mapper.Query<EthTrackerWallet>(sql, new { ids }).ToList();
        }

        public static List<EthTrackerWallet> GetByAddresses(IEnumerable<string> addresses)
        {
            var sql = @"SELECT *
                FROM eth_tracker_wallet
                WHERE address IN @addresses";

            return Db.Mapper.Query<EthTrackerWallet>(sql, new { addresses }).ToList();
        }

        public static void Insert(EthTrackerWallet ethTrackerWallet)
        {
            string sql = @"
				INSERT INTO `eth_tracker_wallet` (
                    `eth_tracker_type_id`,
                    `name`,
					`address`,
                    `is_active`
				) VALUES (
                    @ethTrackerTypeId,
                    @name,
					@address,
                    @isActive
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                ethTrackerTypeId = ethTrackerWallet.EthTrackerTypeId,
                name = ethTrackerWallet.Name?.Trim(),
                address = ethTrackerWallet.Address?.Trim(),
                isActive = ethTrackerWallet.IsActive
            };

            ethTrackerWallet.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }

        public static void LoadTypes(IEnumerable<EthTrackerWallet> ethTrackerWallets)
        {
            Db.LoadEntities(ethTrackerWallets, x => x.EthTrackerTypeId, typeIds => EthTrackerTypes.GetAll(), (ethTrackerWallet, types) => ethTrackerWallet.EthTrackerType = types.FirstOrDefault(t => t.Id == ethTrackerWallet.EthTrackerTypeId));
        }
    }
}
