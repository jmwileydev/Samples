
using System;
using System.Collections.ObjectModel;

namespace ListBoxStylingSample.Interfaces;

public interface ITransactionListViewModel
{
    ObservableCollection<ITransaction> Transactions { get; }

    String[] Categories { get; }
}
