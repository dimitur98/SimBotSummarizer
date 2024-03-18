namespace SimBotUltraSummarizer.Helpers.Log
{
    public interface ILogHelper
    {
        void Error(Exception exception);

        void Info(string message);
    }
}
