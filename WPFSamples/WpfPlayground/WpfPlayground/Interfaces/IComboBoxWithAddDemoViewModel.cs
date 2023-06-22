using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WpfPlayground.Interfaces;

public interface IComboBoxWithAddDemoViewModel : IViewModel
{
    ObservableCollection<string> Items { get; }
    string SelectedItem { get; set; }

    ICommand AddNewItemCommand { get; }
}