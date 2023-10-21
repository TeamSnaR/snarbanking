namespace SnarBanking.Core;

public interface IGenericWriteService<T> where T : class
{
    Task<string> AddOneAsync(T entity);
    Task<long> ReplaceOneAsync(string id, T entity);

    Task<long> DeleteOneAsync(string id);
}
