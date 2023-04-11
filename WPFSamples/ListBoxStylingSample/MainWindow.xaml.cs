using ListBoxStylingSample.ViewModels;
using System.Windows;

namespace ListBoxStylingSample;

public partial class MainWindow : Window
{
    public MainWindow(ITransactionListViewModel transactionListViewModel)
    {
        TransactionListViewModel = transactionListViewModel;
        InitializeComponent();
    }

    public ITransactionListViewModel TransactionListViewModel { get; }
}
