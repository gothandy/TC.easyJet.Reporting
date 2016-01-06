namespace Gothandy.Tables.Interfaces
{
    public interface ICompare<T>
    {
        bool ValueEquals(T other);
        bool KeyEquals(T other);
    }
}
