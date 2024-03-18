namespace SqlQueryBuilder.MySql
{
    public class Limit
    {
        public int? Offset { get; set; }
        public int? RowCount { get; set; }
        public bool IsEmpty { get { return !(this.Offset.HasValue || this.RowCount.HasValue); } }

        public Limit(int? offset = null, int? rowCount = null)
        {
            this.Offset = offset;
            this.RowCount = rowCount;
        }

        public override string ToString()
        {
            if (!this.IsEmpty)
            {
                return string.Format("{0}, {1}", this.Offset ?? 0, this.RowCount ?? int.MaxValue);
            }

            return string.Empty;
        }
    }
}
