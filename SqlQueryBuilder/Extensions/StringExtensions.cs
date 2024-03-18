namespace SqlQueryBuilder.Extensions
{
    public static class StringExtensions
    {
        public static bool StartsWithAny(this string s, IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                if (s.StartsWith(value)) { return true; }
            }

            return false;
        }
    }
}
