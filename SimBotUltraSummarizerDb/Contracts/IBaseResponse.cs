namespace SimBotUltraSummarizerDb.Contracts
{
    public interface IBaseResponse<out T>
    {
        IEnumerable<T> Records { get; }

        long TotalRecords { get; }
    }
}
