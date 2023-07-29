using System.Text.Json.Serialization;

namespace SnarBanking.Expenses;

public record ExpenseDto(
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("amount")] Money Amount,
    [property: JsonPropertyName("category")] string Category,
    [property: JsonPropertyName("store")] string Store,
    [property: JsonPropertyName("purchaseDate")] DateTimeOffset PurchaseDate
);