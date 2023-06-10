using System.Windows;
using WpfPlayground.Interfaces;

namespace WpfPlayground;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(IMainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        MainWindowViewModel = mainWindowViewModel;
    }

    public IMainWindowViewModel MainWindowViewModel { get; }
}
