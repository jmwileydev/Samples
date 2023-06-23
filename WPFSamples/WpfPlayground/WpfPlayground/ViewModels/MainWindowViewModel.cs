using CommunityToolkit.Mvvm.Input;
using JMWToolkit.MVVM.ViewModels;
using System.Diagnostics;
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
        MouseMoveCommand = new RelayCommand<FrameworkElement>(OnMouseMove);
        MouseLeftButtonDownCommand = new RelayCommand<FrameworkElement>(OnMouseLeftButtonDown);
        MouseLeftButtonUpCommand = new RelayCommand<FrameworkElement>(OnMouseLeftButtonUp);

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

    public ICommand MouseLeftButtonDownCommand { get; }

    public ICommand MouseLeftButtonUpCommand { get; }

    public ICommand MouseMoveCommand { get; }

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

    #region WindowMoveRegion
    private string _mousePosition = string.Empty;
    private bool _isMoving = false;
    private Point _moveStartPoint;

    public bool IsMoving
    {
        get => _isMoving;

        set
        {
            if (value != _isMoving)
            {
                _isMoving = value;
                OnPropertyChanged();
            }
        }
    }

    public string MousePosition
    {
        get => _mousePosition;

        set
        {
            if (value != _mousePosition)
            {
                _mousePosition = value;
                OnPropertyChanged();
            }
        }
    }

    public void OnMouseMove(FrameworkElement element)
    {
        var position = Mouse.GetPosition(element);

        if (IsMoving && Mouse.LeftButton == MouseButtonState.Pressed)
        {
            Application.Current.MainWindow.Left += (position.X - _moveStartPoint.X);
            Application.Current.MainWindow.Top += (position.Y - _moveStartPoint.Y);
        }

        MousePosition = $"({position.X}, {position.Y})";
    }

    public void OnMouseLeftButtonDown(FrameworkElement element)
    {
        var position = Mouse.GetPosition(element);
        if (Application.Current.MainWindow.WindowState == WindowState.Normal &&
            position.Y < 10 && position.X > 10 && position.X < (element.ActualWidth - 10))
        {
            numDowns++;
            _moveStartPoint = Mouse.GetPosition(element);

            // If we are clicked in the top few pixels of the Window then show a thin border
            // and change the cursor
            element.CaptureMouse();
            IsMoving = true;
            Debug.WriteLine($"Mouse Down starting capture, numDowns = {numDowns}");
        }
    }
    #endregion

    private int numUps = 0;
    private int numDowns = 0;

    public void OnMouseLeftButtonUp(FrameworkElement element)
    {
        numUps++;
        if (IsMoving)
        {
            IsMoving = false;
            element.ReleaseMouseCapture();
            Debug.WriteLine($"Mouse Up after capture, numUps = {numUps}");

            var position = Mouse.GetPosition(element);
            MousePosition = $"({position.X}, {position.Y})";
        }

    }
}
