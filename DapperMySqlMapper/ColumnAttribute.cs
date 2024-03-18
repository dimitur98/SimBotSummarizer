namespace DapperMySqlMapper
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
        string name;
        string storage;

        public ColumnAttribute() { }

        public string Name
        {
            get { return this.name; }
            set { name = value; }
        }

        public string Storage
        {
            get { return this.storage; }
            set { this.storage = value; }
        }
    }
}
