
using ListBoxStylingSample.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace ListBoxStylingSample.ViewModels;

public class TransactionListViewModel : ITransactionListViewModel
{
    public String[] Categories { get; } = new String[]
    {
        "Dining/Entertainment",
        "Food",
        "Household",
        "Miscellaneous",
        "Transportation"
    };

    public ObservableCollection<ITransaction> Transactions { get; private set; }

    public TransactionListViewModel()
    {
        Transactions = new ObservableCollection<ITransaction>()
        {
            new TransactionViewModel(Convert.ToDateTime("3/1/2023"), "Wegmans", 24.03, "Food"),
            new TransactionViewModel(Convert.ToDateTime("3/2/2023"), "Chick-Filet", 10.02, "Dining/Entertainment"),
            new TransactionViewModel(Convert.ToDateTime("3/3/2023"), "Exxon", 58.35, "Transportation"),
            new TransactionViewModel(Convert.ToDateTime("3/4/2023"), "Target", 120.00,"Household"),
            new TransactionViewModel(Convert.ToDateTime("3/4/2023"), "Harris Teeter", 19.57, "Food"),
            new TransactionViewModel(Convert.ToDateTime("3/6/2023"), "TruValue", 78.25, "Household"),
            new TransactionViewModel(Convert.ToDateTime("3/7/2023"), "Yardhouse", 87.22, "Dining/Entertainment"),
            new TransactionViewModel(Convert.ToDateTime("3/8/2023"), "Wegmans", 37.23, "Food"),
            new TransactionViewModel(Convert.ToDateTime("3/19/2023"), "Costco", 78.27, "Food")
        };
    }

}
