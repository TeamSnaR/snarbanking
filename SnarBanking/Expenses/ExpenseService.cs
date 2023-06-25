using MongoDB.Driver;

using static SnarBanking.Storage.Service;
using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Expenses;

public interface IGenericService<T> where T : class
{
    Task<IReadOnlyList<T>> GetAsync(
        IFilterDefinitionSpecification<T> specification
    );
    Task<IReadOnlyList<TNewProjection>> GetAsync<TNewProjection>(
        IFilterDefinitionSpecification<T> specification,
        IProjector<T, TNewProjection> projector
    );
    Task<T> GetOneAsync(
        IFilterDefinitionSpecification<T> specification
    );
}
public class ExpenseService : IGenericService<Expense>
{
    private readonly SnarBankingMongoDbService _snarBankingMongoDbService;

    public ExpenseService(SnarBankingMongoDbService snarBankingMongoDbService)
    {
        _snarBankingMongoDbService = snarBankingMongoDbService;
    }

    public Task<IReadOnlyList<TNewProjection>> GetAsync<TNewProjection>(IFilterDefinitionSpecification<Expense> specification, IProjector<Expense, TNewProjection> projector) =>
        _snarBankingMongoDbService.ExpensesCollection
            .Find(specification.IsSatisfiedBy())
            .Project(projector.ProjectAs())
            .ToListAsync()
                .ContinueWith<IReadOnlyList<TNewProjection>>(list => list.Result);

    public Task<IReadOnlyList<Expense>> GetAsync(IFilterDefinitionSpecification<Expense> specification) =>
        _snarBankingMongoDbService.ExpensesCollection
            .FindAsync(specification.IsSatisfiedBy())
                .ContinueWith<IReadOnlyList<Expense>>(list => list.Result.ToList());

    public Task<Expense> GetOneAsync(IFilterDefinitionSpecification<Expense> specification) => _snarBankingMongoDbService.ExpensesCollection
            .FindAsync(specification.IsSatisfiedBy())
                .ContinueWith(list => list.Result.FirstOrDefault());
}
