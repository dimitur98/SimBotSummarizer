using NLog;

namespace SimBotUltraSummarizer.Helpers.Log
{
    public class LogHelper : ILogHelper
    {
        private readonly Logger Log = LogManager.GetLogger("*");

        public void Error(Exception exception)
        {
            this.Log.Error(exception, exception.Message);
        }

        public void Info(string message) => this.Log.Info(message);
    }
}
