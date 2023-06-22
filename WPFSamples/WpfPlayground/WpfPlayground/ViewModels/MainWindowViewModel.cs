using CommunityToolkit.Mvvm.Input;
using JMWToolkit.MVVM.ViewModels;
using System.Windows;
using System.Windows.Input;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private INavigationService _navigationService;
    private WindowState _currentWindowState = WindowState.Normal;

    public MainWindowViewModel(INavigationService navigation)
    {
        _navigationService = navigation;
        CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
        MinimizeCommand = new RelayCommand(()=> CurrentWindowState = WindowState.Minimized);
        MaximizeCommand = new RelayCommand(() => CurrentWindowState = WindowState.Maximized);
        OverlapCommand = new RelayCommand(() => CurrentWindowState = WindowState.Normal);
        NavigateToAnimationDemo = new RelayCommand(() => { NavigationService.NavigateTo<IAnimationDemoViewModel>(); });
        NavigateToComboBoxWithAddDemo = new RelayCommand(() => { NavigationService.NavigateTo<IComboBoxWithAddDemoViewModel>(); });
        NavigateToMessageBoxDemo = new RelayCommand(() => { NavigationService.NavigateTo<IMessageBoxDemoAreaViewModel>(); });
        NavigationService.NavigateTo<IMessageBoxDemoAreaViewModel>();
    }

    public INavigationService NavigationService
    {
        get => _navigationService;

        set
        {
            if (value != _navigationService)
            {
                _navigationService = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand CloseCommand { get; }

    public ICommand MinimizeCommand { get; }

    public ICommand MaximizeCommand { get; }

    public ICommand OverlapCommand { get;  }

    public ICommand NavigateToMessageBoxDemo { get; }

    public ICommand NavigateToComboBoxWithAddDemo { get; }

    public ICommand NavigateToAnimationDemo{ get; }

    public WindowState CurrentWindowState
    {
        get
        {
            return _currentWindowState;
        }

        set
        {
            if (value != _currentWindowState)
            {
                _currentWindowState = value;
                OnPropertyChanged();
            }
        }
    }
}
