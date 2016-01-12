namespace Gothandy.Tables.Interfaces
{
    public interface ITableWrite<T> : ITableRead<T>
    {
        void BatchInsert(T item);
        void BatchReplace(T item);
        void BatchInsertOrReplace(T item);
        void BatchDelete(T item);
        void BatchComplete();
    }
}
