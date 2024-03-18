using SimBotUltraSummarizer.Helpers.Log;

namespace SimBotUltraSummarizer.Helpers.Extensions
{
    public static class ExceptionExtensions
    {
        private static readonly LogHelper Logger = new();

        public static void SaveToLog(this Exception ex)
        {
            ExceptionExtensions.Logger.Error(ex);
        }
    }
}
