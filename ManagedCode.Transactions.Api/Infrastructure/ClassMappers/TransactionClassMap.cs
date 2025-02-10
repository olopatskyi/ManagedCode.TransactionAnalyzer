using System.Globalization;
using CsvHelper.Configuration;
using ManagedCode.Transactions.Api.Models;

namespace ManagedCode.Transactions.Api.Infrastructure.ClassMappers;

public sealed class TransactionClassMap : ClassMap<TransactionModel>
{
    public TransactionClassMap()
    {
        Map(m => m.TransactionId).Name("TransactionId");
        Map(m => m.UserId).Name("UserId");
        Map(m => m.Date).Name("Date").TypeConverterOption.Format("yyyy-MM-ddTHH:mm:ss");
        Map(m => m.Amount).Name("Amount").TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture);
        Map(m => m.Category).Name("Category");
        Map(m => m.Description).Name("Description");
        Map(m => m.Merchant).Name("Merchant");
    }
}