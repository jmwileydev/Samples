using BezierCurvePlay.Interfaces;

namespace BezierCurvePlay.ViewModels;

public class MainWindowViewModel : IMainWindowViewModel
{
    public IBezierCurveViewModel BezierCurveViewModel { get; private set; }

    public MainWindowViewModel(IBezierCurveViewModel bezierCurveViewModel)
    {
        BezierCurveViewModel = bezierCurveViewModel;
    }
}
