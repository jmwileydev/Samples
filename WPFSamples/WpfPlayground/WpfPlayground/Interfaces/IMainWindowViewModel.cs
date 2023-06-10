using System.Collections.ObjectModel;

namespace WpfPlayground.Interfaces;

public interface IMainWindowViewModel
{
    ObservableCollection<string> Items { get; }

    public string SelectedItem { get; set; }
}