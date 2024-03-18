using SqlQueryBuilder.Extensions;
using System.Text;

namespace SqlQueryBuilder.MySql
{
    public class Query
    {
        public List<string> Select { get; set; }
        public string From { get; set; }
        public List<string> Joins { get; set; }
        public List<string> Where { get; set; }
        public List<string> GroupBy { get; set; }
        public List<string> Having { get; set; }
        public bool GroupByWithRollup { get; set; }
        public List<OrderBy> OrderBys { get; set; }
        public Limit Limit { get; set; }

        public Query()
        {

        }

        public Query(IEnumerable<string> select, string from, IEnumerable<string> joins = null, IEnumerable<string> where = null, IEnumerable<string> groupBy = null, IEnumerable<string> having = null, IEnumerable<OrderBy> orderBys = null, Limit limit = null)
        {
            this.Select = select.ToList();
            this.From = from;
            this.Joins = joins != null ? joins.ToList() : null;
            this.Where = where != null ? where.ToList() : null;
            this.GroupBy = groupBy != null ? groupBy.ToList() : null;
            this.Having = having != null ? having.ToList() : null;
            this.OrderBys = orderBys != null ? orderBys.ToList() : null;
            this.Limit = limit;
        }

        public string Build()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT\n    ");
            sql.Append(String.Join(",\n    ", this.Select));
            sql.Append("\nFROM ");
            sql.Append(this.From);

            if (this.Joins != null && this.Joins.Any())
            {
                sql.Append("\n    ");
                sql.Append(String.Join("\n    ", this.Joins));
            }

            if (this.Where != null && this.Where.Any())
            {
                sql.Append("\nWHERE ");
                sql.Append(String.Join("\n    ", this.Where));
            }

            if (this.GroupBy != null && this.GroupBy.Any())
            {
                sql.Append("\nGROUP BY ");
                sql.Append(String.Join(", ", this.GroupBy));

                if (this.GroupByWithRollup == true)
                {
                    sql.Append(" WITH ROLLUP");
                }
            }
            else
            {
                this.GroupByWithRollup = false;
            }

            if (this.Having != null && this.Having.Any())
            {
                sql.Append("\nHAVING ");
                sql.Append(String.Join("\n    ", this.Having));
            }

            if (this.OrderBys != null)
            {
                this.OrderBys = this.OrderBys.Where(o => o.IsValid || o.Column.ToCharArray().All(c => Char.IsLetter(c) || Char.IsNumber(c) || c == '.' || c == '_' || c == ' ' || c == '(' || c == ')' || c == '`')).ToList();
            }

            if (this.OrderBys != null && this.OrderBys.Any() && this.GroupByWithRollup == false)
            {
                sql.Append("\nORDER BY ");
                sql.Append(string.Join(", ", this.OrderBys.Select(o => o.ToString())));
            }

            if (this.Limit != null && !this.Limit.IsEmpty)
            {
                sql.AppendFormat("\nLIMIT {0}", this.Limit.ToString());
            }

            return sql.ToString();
        }

        public void AddJoinIf(string join, string orderByColumnStartsWith = null, bool? condition = null)
        {
            if (condition == true || (orderByColumnStartsWith != null && this.OrderBys.Any(o => o.Column.StartsWith(orderByColumnStartsWith))))
            {
                this.Joins.Add(join);
            }
        }

        public void AddJoinIf(string join, IEnumerable<string> orderByColumnStartsWith = null, bool? condition = null, IEnumerable<string> orderByColumnContains = null)
        {
            if (condition == true || (orderByColumnStartsWith != null && this.OrderBys.Any(o => o.Column.StartsWithAny(orderByColumnStartsWith))))
            {
                this.Joins.Add(join);

                return;
            }

            if (orderByColumnContains != null)
            {
                foreach (var searchFor in orderByColumnContains)
                {
                    if (this.OrderBys.Any(o => o.Column.IndexOf(searchFor) > -1))
                    {
                        this.Joins.Add(join);

                        return;
                    }
                }
            }
        }
    }
}
