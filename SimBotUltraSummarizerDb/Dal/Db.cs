using DapperMySqlMapper;
using SqlQueryBuilder.MySql;
using System.Data;
using System.Reflection;
using SimBotSummarizer.Helpers.Extensions;

namespace SimBotUltraSummarizerDb.Dal
{
    public static class Db 
    {
        public static MySqlMapper Mapper { get; set; }

        public static void SetupConnection(string connectionString, int? unableToConnectToHostErrorRetryInterval = null, bool unableToConnectToHostErrorRetryTillConnect = false)
        {
            Db.Mapper = new MySqlMapper(connectionString, unableToConnectToHostErrorRetryInterval: unableToConnectToHostErrorRetryInterval, unableToConnectToHostErrorRetryTillConnect: unableToConnectToHostErrorRetryTillConnect);
        }

        public static List<string> GetColumnNames<T>(string tableAlias = "", string columnAliasPrefix = "", string spliter = "")
        {
            var columnNames = new List<string>();
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();

            if (!String.IsNullOrWhiteSpace(tableAlias) && tableAlias.IndexOf('.') <= -1) { tableAlias += "."; }

            foreach (var property in properties)
            {
                var columnAttribute = (ColumnAttribute)Attribute.GetCustomAttribute(property, typeof(ColumnAttribute));

                if (columnAttribute != null && !String.IsNullOrEmpty(columnAttribute.Name))
                {
                    columnNames.Add(!String.IsNullOrWhiteSpace(columnAliasPrefix) ? String.Format("{0}{1} AS {2}{1}", tableAlias, columnAttribute.Name, columnAliasPrefix) : tableAlias + columnAttribute.Name);
                }
            }

            if (!String.IsNullOrWhiteSpace(spliter))
            {
                columnNames.Add(spliter);
            }

            return columnNames;
        }

        public static string GetColumnName<T>(string propertyName, string tableAlias = "", string columnAliasPrefix = "")
        {
            if (!String.IsNullOrWhiteSpace(tableAlias) && tableAlias.IndexOf('.') <= -1) { tableAlias += "."; }

            var property = typeof(T).GetProperty(propertyName);
            var columnAttribute = (ColumnAttribute)Attribute.GetCustomAttribute(property, typeof(ColumnAttribute));

            if (columnAttribute != null && !String.IsNullOrEmpty(columnAttribute.Name))
            {
                return !String.IsNullOrWhiteSpace(columnAliasPrefix) ? String.Format("{0}{1} AS {2}{1}", tableAlias, columnAttribute.Name, columnAliasPrefix) : tableAlias + columnAttribute.Name;
            }

            return null;
        }

        public static void LoadEntities<TEntity, TPKeyEntity,T>(IEnumerable<TEntity> entities,
            Func<TEntity, T> pKeySelector,
            Func<IEnumerable<T>, ICollection<TPKeyEntity>> pKeyEntitiesSelector,
            Action<TEntity, ICollection<TPKeyEntity>> map)
            where TEntity : class
            where TPKeyEntity : class
        {
            if (!entities.HasItems()) { return; }

            var pKeyIds = entities.Select(pKeySelector).Distinct().ToList();

            if (!pKeyIds.HasItems()) { return; }

            var pKeyEntities = pKeyEntitiesSelector(pKeyIds);

            foreach (var entity in entities)
            {
                map(entity, pKeyEntities);
            }
        }
        public static void LoadEntities<TEntity, TPKeyEntity>(IEnumerable<TEntity> entities,
            Func<TEntity, int> pKeySelector,
            Func<IEnumerable<int>, ICollection<TPKeyEntity>> pKeyEntitiesSelector,
            Action<TEntity, ICollection<TPKeyEntity>> map)
            where TEntity : class
            where TPKeyEntity : class
        {
            if (!entities.HasItems()) { return; }

            var pKeyIds = entities.Select(pKeySelector).Distinct().ToList();

            if (!pKeyIds.HasItems()) { return; }

            var pKeyEntities = pKeyEntitiesSelector(pKeyIds);

            foreach (var entity in entities)
            {
                map(entity, pKeyEntities);
            }
        }

        public static long QueryCount(IDbConnection connection, Query query, object param = null)
        {
            var totalsQuery = new Query(new string[] { "COUNT(*)" }, query.From, joins: query.Joins, where: query.Where, groupBy: query.GroupBy);
            var count = Db.Mapper.Query<long?>(connection, totalsQuery.Build(), param: param).FirstOrDefault();

            return count == null ? 0 : count.Value;
        }

        internal static bool ExistsByField(string tableName, string fieldName, object fieldValue, string idFieldName, object ignoreId = null)
        {
            string sql = $@"
                SELECT 1
                FROM {tableName} 
                WHERE {fieldName} = @fieldValue";

            if (ignoreId != null) { sql += $" AND {idFieldName} <> @ignoreId"; }

            return Db.Mapper.Query<int>(sql, param: new { fieldValue, ignoreId }).FirstOrDefault() == 1;
        }

        internal static bool ExistsByField(string tableName, string fieldName, string fieldValue, string idFieldName, object ignoreId = null, bool trimFieldValue = true)
        {
            if (trimFieldValue) { fieldValue = fieldValue?.Trim(); }

            string sql = $@"
                SELECT 1
                FROM {tableName} 
                WHERE {fieldName} = @fieldValue";

            if (ignoreId != null) { sql += $" AND {idFieldName} <> @ignoreId"; }

            return Db.Mapper.Query<int>(sql, param: new { fieldValue, ignoreId }).FirstOrDefault() == 1;
        }
        internal static bool ExistsByField(string tableName, string fieldName, string fieldValue, string idFieldName, string idFieldName2, object idFieldName2EqualTo, object ignoreId = null, bool trimFieldValue = true )
        {
            if (trimFieldValue) { fieldValue = fieldValue?.Trim(); }
            
            string sql = $@"
                SELECT 1
                FROM {tableName} 
                WHERE {fieldName} = @fieldValue";

            if (ignoreId != null) { sql += $" AND {idFieldName} <> @ignoreId"; }
            if (idFieldName2EqualTo != null) { sql += $" AND {idFieldName2} = @idFieldName2EqualTo"; }

            return Db.Mapper.Query<int>(sql, param: new { fieldValue, ignoreId, idFieldName2EqualTo }).FirstOrDefault() == 1;
        }

        internal static bool ExistsByFields(string tableName, IDictionary<string, object> fields, IDictionary<string, object> ignoreFields = null)
        {
            string sql = $@"
                SELECT 1
                FROM {tableName} 
                WHERE 1 = 1";

            var param = new Dictionary<string, object>(fields);

            foreach (var pair in fields )
            {
                if (pair.Value != null)
                {
                    sql += $" AND `{pair.Key}` = @{pair.Key}\n\r";
                }
                else
                {
                    sql += $" AND `{pair.Key}` IS NULL\n\r";
                }
            }

            if (ignoreFields != null)
            {
                foreach (var pair in ignoreFields)
                {
                    if (pair.Value != null)
                    {
                        sql += $" AND `{pair.Key}` <> @ignore_{pair.Key}\n\r";

                        param.Add("ignore_" + pair.Key, pair.Value);
                    }
                }
            }

            return Db.Mapper.Query<int>(sql, param: param).FirstOrDefault() == 1;
        }

        internal static void DeleteByField<TValue>(string tableName, string field, TValue value)
        {
            string sql = $@"
                DELETE FROM {tableName} 
                WHERE {field} = @value";

            Db.Mapper.Execute(sql, param: new { value });
        }
    }    
}
