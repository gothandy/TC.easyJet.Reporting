using Microsoft.WindowsAzure.Storage.Table;

namespace Gothandy.Tables.AzureTables.Interfaces
{
    public interface IConverter<T,U> where U : TableEntity
    {
        T Read(U azureEntity);
        U Write(T dataEntity);
    }
}
