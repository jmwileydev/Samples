using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Animation;
using WpfPlayground.ViewModels;

namespace WpfPlayground.Interfaces;

public interface IAnimationDemoViewModel : IViewModel
{
    public ICommand BeginAnimationCommand
    {
        get;
    }

    public int Bounces { get; set; }
    public double Bounciness { get; set; }
    public EasingFunction EasingFunction { get; set; }
    public EasingMode EasingMode { get; set; }
    public ObservableCollection<EasingMode> EasingModes { get; }
    public ObservableCollection<EasingFunction> EasingFunctions { get; }
}
