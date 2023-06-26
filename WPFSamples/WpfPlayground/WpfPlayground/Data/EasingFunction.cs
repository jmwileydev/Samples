using System;
using WpfPlayground.Controls;

namespace WpfPlayground.ViewModels;

/// <summary>
/// Delegate to call to begin an animation
/// </summary>
/// <param name="border"></param>
public delegate void RaiseEventDelegate(AnimatedBorder border);

/// <summary>
/// Record to hold the values needed for the Easing Functions
/// </summary>
public record class EasingFunction
{
    public Type Type { get; init; }
    public bool Bounces { get; init; }
    public RaiseEventDelegate RaiseEventDelegate { get; init; }
}
