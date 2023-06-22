using BezierCurvePlay.Interfaces;

namespace BezierCurvePlay.ViewModels;

public class BezierCurveViewModel : IBezierCurveViewModel
{
    public int Width { get; set; } = 400;
    public int Height { get; set; } = 400;
}
