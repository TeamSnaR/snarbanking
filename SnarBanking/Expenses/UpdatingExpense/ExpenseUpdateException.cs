namespace SnarBanking.Expenses.UpdatingExpense;

internal class ExpenseUpdateException : Exception
{
    internal ExpenseUpdateException(string message) : base(message)
    {
    }
}