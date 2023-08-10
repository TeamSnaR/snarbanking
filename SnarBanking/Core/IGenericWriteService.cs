namespace SnarBanking.Core;

public interface IGenericWriteService<T> where T : class
{
    Task<string> AddOneAsync(T entity);
    Task UpdateOneAsync(string id, T entity);
}
