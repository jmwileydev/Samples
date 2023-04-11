using System;

namespace ListBoxStylingSample.ViewModels;

public interface ITransaction
{
    DateTime Date { get; }
    String Description { get; }

    double Amount { get; }

    String Category { get; }
}

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
