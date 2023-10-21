using MediatR;

using SnarBanking.Core;
using SnarBanking.Expenses;

internal static class DeleteExpense
{
    internal record Command(string Id) : IRequest;

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IGenericWriteService<Expense> _genericWriteService;

        public Handler(IGenericWriteService<Expense> genericWriteService)
        {
            _genericWriteService = genericWriteService;
        }
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _genericWriteService.DeleteOneAsync(request.Id);
        }
    }
}