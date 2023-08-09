using MongoDB.Driver;

using SnarBanking.Core;

using static SnarBanking.Storage.Service;
using static SnarBanking.Storage.Specifications;

namespace SnarBanking.Expenses;

public interface IGenericService<T> where T : class
{
    Task<IReadOnlyList<T>> GetManyAsync(
        IFilterDefinitionSpecification<T> specification
    );
    Task<IReadOnlyList<TNewProjection>> GetManyAsync<TNewProjection>(
        IFilterDefinitionSpecification<T> specification,
        IProjector<T, TNewProjection> projector
    );
    Task<TNewProjection> GetOneAsync<TNewProjection>(
        IFilterDefinitionSpecification<T> specification,
        IProjector<T, TNewProjection> projector
    );

    Task<T> GetOneAsync(
        IFilterDefinitionSpecification<T> specification
    );
}
public class ExpenseService : IGenericService<Expense>, IGenericWriteService<Expense>
{
    private readonly SnarBankingMongoDbService _snarBankingMongoDbService;

    public ExpenseService(SnarBankingMongoDbService snarBankingMongoDbService)
    {
        _snarBankingMongoDbService = snarBankingMongoDbService;
    }

    public Task<TNewProjection> GetOneAsync<TNewProjection>(IFilterDefinitionSpecification<Expense> specification, IProjector<Expense, TNewProjection> projector) =>
        _snarBankingMongoDbService.ExpensesCollection
            .Find(specification.IsSatisfiedBy())
            .Project(projector.ProjectAs())
            .FirstOrDefaultAsync()
            .ContinueWith(list => list.Result);

    public Task<IReadOnlyList<Expense>> GetManyAsync(IFilterDefinitionSpecification<Expense> specification) =>
        _snarBankingMongoDbService.ExpensesCollection
            .FindAsync(specification.IsSatisfiedBy())
                .ContinueWith<IReadOnlyList<Expense>>(list => list.Result.ToList());

    public Task<Expense> GetOneAsync(IFilterDefinitionSpecification<Expense> specification) => _snarBankingMongoDbService.ExpensesCollection
            .FindAsync(specification.IsSatisfiedBy())
                .ContinueWith(list => list.Result.FirstOrDefault());

    public Task<IReadOnlyList<TNewProjection>> GetManyAsync<TNewProjection>(IFilterDefinitionSpecification<Expense> specification, IProjector<Expense, TNewProjection> projector) =>
        _snarBankingMongoDbService.ExpensesCollection
            .Find(specification.IsSatisfiedBy(), new FindOptions())
            .Project(projector.ProjectAs())
            .ToListAsync()
            .ContinueWith<IReadOnlyList<TNewProjection>>(list => list.Result);

    public Task<string> AddOneAsync(Expense expense) =>
        _snarBankingMongoDbService.ExpensesCollection
            .InsertOneAsync(expense)
            .ContinueWith<string>(task => expense.Id);

    public Task<string> AddManyAsync(IEnumerable<Expense> entities)
    {
        throw new NotImplementedException();
    }
}
