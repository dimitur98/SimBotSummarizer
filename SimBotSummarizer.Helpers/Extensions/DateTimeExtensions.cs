namespace SimBotSummarizer.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToWebDateFormat(this DateTime dt, bool showTime = false)
        {
            return ToWebDateFormat(dt, showTime, String.Empty);
        }

        public static string ToWebDateFormat(this DateTime dt, bool showTime, string defaultText)
        {
            if (dt > DateTime.MinValue)
            {
                string format = "dd MMM yyyy";
                if (showTime) { format += " " + "HH:mm"; }

                return dt.ToString(format);
            }

            return defaultText;
        }
    }
}
