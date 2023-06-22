using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace WpfPlayground.Interfaces;

public interface IMainWindowViewModel : IViewModel
{
    public ICommand CloseCommand { get; }
    public ICommand MinimizeCommand { get; }
    public ICommand MaximizeCommand { get; }

    public ICommand OverlapCommand { get; }

    public WindowState CurrentWindowState { get; set; }
}