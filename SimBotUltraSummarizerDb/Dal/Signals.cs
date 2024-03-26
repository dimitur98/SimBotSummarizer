using SimBotUltraSummarizerDb.Models;
using SimBotUltraSummarizerDb.Models.SignalsSearch;
using SqlQueryBuilder.MySql;
using System.Data;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class Signals
    {
        public static Response Search(Request request)
        {
            var query = new Query
            {
                Select = new List<string>() { "s.*" },
                From = "`signal` AS s",
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

            if (!string.IsNullOrEmpty(request.Keywords)) { query.Where.Add("AND (s.name LIKE @keywords)"); }
            if (request.StartDate.HasValue) { query.Where.Add(" AND  sd.date >= @startDate"); }
            if (request.EndDate.HasValue) { query.Where.Add(" AND sd.date <= @endDate"); }
            if (request.MCapFrom.HasValue) { query.Where.Add(" AND sd.mcap >= @mcapFrom"); }
            if (request.MCapTo.HasValue) { query.Where.Add(" AND sd.mcap <= @mcapTo"); }
            if (request.TotalCallsFrom.HasValue) { query.Where.Add(" AND sd.total_calls >= @totalCallsFrom"); }
            if (request.TotalCallsTo.HasValue) { query.Where.Add(" AND sd.total_calls <= @totalCallsTo"); }
            if (request.HasHypeAlarmSignal.HasValue) { query.Where.Add($" AND {(request.HasHypeAlarmSignal.Value ? string.Empty : "NOT")} EXISTS(SELECT * FROM hype_signal hs WHERE hs.address = s.address)"); }
            if (request.HasITokenSignal.HasValue) { query.Where.Add($" AND {(request.HasITokenSignal.Value ? string.Empty : "NOT")} EXISTS(SELECT * FROM itoken i WHERE i.address = s.address)"); }

            query.Joins.Add(" LEFT JOIN signal_data sd ON sd.id = (SELECT sd2.id FROM signal_data as sd2 WHERE sd2.signal_id = s.id ORDER BY sd2.date LIMIT 1)");

            query.Limit = new Limit(request.Offset, request.RowCount);

            var response = new Response();

            using (var connection = Db.Mapper.GetConnection())
            {
                var queryParams = new
                {
                    keywords = string.Format("%{0}%", request.Keywords),
                    startDate = request.StartDate, 
                    endDate = request.EndDate,
                    mcapFrom = request.MCapFrom,
                    mcapTo = request.MCapTo,
                    totalCallsFrom = request.TotalCallsFrom,
                    totalCallsTo = request.TotalCallsTo
                };

                //get TotalRecordsCount
                if (request.ReturnTotalRecords)
                {
                    response.TotalRecords = Db.QueryCount(connection, query, param: queryParams);

                    if (response.TotalRecords <= 0) { return response; }
                }

                response.Records = Db.Mapper.Query<Signal>(connection, query.Build(), queryParams, commandType: CommandType.Text);
            }

            return response;
        }

        public static void Insert(Signal signal)
        {
            string sql = @"
				INSERT INTO `signal` (
					`name`,
                    `address`,
                    `pair`
				) VALUES (
					@name,
                    @address,
                    @pair
				);

				SELECT LAST_INSERT_ID() AS id;";

            var queryParams = new
            {
                name = signal.Name.Trim(),
                address = signal.Address.Trim(),
                pair = signal.Pair
            };

            signal.Id = Db.Mapper.Query<uint>(sql, param: queryParams).First();
        }

        public static void LoadSignalData(IEnumerable<Signal> signals)
        {
            Db.LoadEntities(signals, x => x.Address, addresses => SignalDatas.GetByAddress(addresses), (signal, signalDatas) => signal.SignalData = signalDatas.Where(x => x.Address == signal.Address).ToList());
        }

        public static void LoadHypeSignals(IEnumerable<Signal> signals)
        {
            Db.LoadEntities(signals, x => x.Address, addresses => HypeSignals.GetByAddress(addresses), (signal, hypeSignals) => signal.HypeSignals = hypeSignals.Where(x => x.Address == signal.Address).ToList());
        }

        public static void LoadITokenSignals(IEnumerable<Signal> signals)
        {
            Db.LoadEntities(signals, x => x.Address, addresses => ITokens.GetByAddress(addresses), (signal, itokens) => signal.ITokens = itokens.Where(x => x.Address == signal.Address).ToList());
        }
    }
}
