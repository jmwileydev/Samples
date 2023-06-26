using JMWToolkit.MVVM.ViewModels;
using System.Windows.Media.Animation;
using WpfPlayground.Interfaces;
using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WpfPlayground.Controls;

namespace WpfPlayground.ViewModels;

/// <summary>
/// ViewModel for the Animation Demo Section
/// </summary>
public class AnimationDemoViewModel : ViewModelBase, IAnimationDemoViewModel
{
    private Storyboard _runningStoryboard = null;
    private int _bounces = 2;
    private double _bounciness = 2.0;
    private EasingMode _easingMode;
    private EasingFunction _easingFunction;

    public AnimationDemoViewModel()
    {
        BeginAnimationCommand = new RelayCommand<AnimatedBorder>((e) =>
        {
            EasingFunction.RaiseEventDelegate(e);
        });

        EasingModes = new ObservableCollection<EasingMode>()
        {
            EasingMode.EaseIn,
            EasingMode.EaseOut,
            EasingMode.EaseInOut
        };

        EasingFunctions = new ObservableCollection<EasingFunction>()
        {
            new EasingFunction()
            {
                Type = typeof(BounceEase),
                Bounces = true,
                RaiseEventDelegate = (b) =>
                {
                    b.RaiseBeginBounceEaseRoutedEvent();
                }
            },
            new EasingFunction()
            {
                Type = typeof(CubicEase),
                Bounces = false,
                RaiseEventDelegate = (b) =>
                {
                    b.RaiseBeginCubicEaseRoutedEvent();
                }
            },
            new EasingFunction()
            {
                Type = typeof(QuarticEase),
                Bounces = false,
                RaiseEventDelegate = (b) =>
                {
                    b.RaiseBeginQuarticEaseRoutedEvent();
                }
            }
        };

        _easingFunction = EasingFunctions[0];
        _easingMode = EasingModes[0];
    }

    private void StoryboardCompleted(object sender, EventArgs e)
    {
        if (_runningStoryboard != null)
        {
            _runningStoryboard = null;
        }
    }

    public ICommand BeginAnimationCommand { get; private set; }

    public int Bounces
    {
        get => _bounces;

        set
        {
            if (value != _bounces)
            {
                _bounces = value;
                OnPropertyChanged();
            }
        }
    }

    public double Bounciness
    {
        get => _bounciness;

        set
        {
            if (value != _bounciness)
            {
                _bounciness = value;
                OnPropertyChanged();
            }
        }
    }
    public EasingMode EasingMode
    {
        get => _easingMode;
        set
        {
            if (value != _easingMode)
            {
                _easingMode = value;
                OnPropertyChanged();
            }
        }
    }

    public EasingFunction EasingFunction
    {
        get => _easingFunction;
        set
        {
            if (value != _easingFunction)
            {
                _easingFunction = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<EasingMode> EasingModes { get; private set; }
    public ObservableCollection<EasingFunction> EasingFunctions { get; private set; }
}
