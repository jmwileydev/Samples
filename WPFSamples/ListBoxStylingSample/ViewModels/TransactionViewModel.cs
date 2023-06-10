using ListBoxStylingSample.Interfaces;
using System;

namespace ListBoxStylingSample.ViewModels;

public class TransactionViewModel : ViewModelBase, ITransaction
{
    public TransactionViewModel(DateTime date, String description, double amount, String category)
    {
        Date = date;
        Description = description;
        Amount = amount;
        Category = category;
    }
    public DateTime Date { get; }

    public string Description { get; }

    public double Amount { get; } = 0.0;

    public string Category { get; }
}
