using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnarBanking.Expenses
{
    public class Expense
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public DateTimeOffset PurchaseDate { get; set; }
        public string? Description { get; set; } = default!;
        public Money Amount { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Store { get; set; } = default!;

        public ICollection<ExpenseItem> Items { get; private set; }

        private Expense()
        {
            Items = new List<ExpenseItem>();
        }

        public Expense(string description, Money amount, string category, string store, DateTimeOffset purchaseDate) : this()
        {
            Description = description;
            Amount = amount;
            Category = category;
            Store = store;
            PurchaseDate = purchaseDate;
        }

        public void AddExpenseItem(ExpenseItem item)
        {
            Items.Add(item);
        }
    }

    public class ExpenseItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Money PricePerUnit { get; set; } = default!;
        public float Quantity { get; set; } = 1;
        public string SubCategory { get; set; } = default!;
        public UnitOfMeasure UnitOfMeasture { get; set; } = default!;
    }

    public record Money
    {
        private Money() { }
        public Money(Currency currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Currency Currency { get; init; }
        public decimal Value { get; init; }
    }

    public enum UnitOfMeasure
    {
        Piece,
        Kilo,
        Pound,
        Liter,
        Box
    }

    public enum Currency
    {
        GBP,
        MYR,
        USD,
        PHP
    }
}

