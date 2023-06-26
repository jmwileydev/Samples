using System.Windows;
using System.Windows.Controls;

namespace WpfPlayground.Controls;

/// <summary>
/// For the Animation Demo, this is a border which will have new RoutedEvents to start a particular
/// animation.
/// </summary>
public class AnimatedBorder : Border
{
    // Register custom routed events using the Bubble routing strategy to begin the animations
    public static readonly RoutedEvent BeginBounceEaseEvent = EventManager.RegisterRoutedEvent(
        name: "BeginBounceEase",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(AnimatedBorder));

    public static readonly RoutedEvent BeginCubicEaseEvent = EventManager.RegisterRoutedEvent(
        name: "BeginCubicEase",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(AnimatedBorder));

    public static readonly RoutedEvent BeginQuarticEaseEvent = EventManager.RegisterRoutedEvent(
        name: "BeginQuarticEase",
        routingStrategy: RoutingStrategy.Bubble,
        handlerType: typeof(RoutedEventHandler),
        ownerType: typeof(AnimatedBorder));

    // Provide CLR accessors for assigning an event handler.
    public event RoutedEventHandler BeginBounceEase
    {
        add { AddHandler(BeginBounceEaseEvent, value); }
        remove { RemoveHandler(BeginBounceEaseEvent, value); }
    }

    // Provide CLR accessors for assigning an event handler.
    public event RoutedEventHandler BeginCubicEase
    {
        add { AddHandler(BeginCubicEaseEvent, value); }
        remove { RemoveHandler(BeginCubicEaseEvent, value); }
    }

    // Provide CLR accessors for assigning an event handler.
    public event RoutedEventHandler BeginQuarticBounceEase
    {
        add { AddHandler(BeginQuarticEaseEvent, value); }
        remove { RemoveHandler(BeginQuarticEaseEvent, value); }
    }

    public void RaiseBeginBounceEaseRoutedEvent()
    {
        // Create a RoutedEventArgs instance.
        RoutedEventArgs routedEventArgs = new(routedEvent: BeginBounceEaseEvent);

        // Raise the event, which will bubble up through the element tree.
        RaiseEvent(routedEventArgs);
    }

    public void RaiseBeginCubicEaseRoutedEvent()
    {
        // Create a RoutedEventArgs instance.
        RoutedEventArgs routedEventArgs = new(routedEvent: BeginCubicEaseEvent);

        // Raise the event, which will bubble up through the element tree.
        RaiseEvent(routedEventArgs);
    }

    public void RaiseBeginQuarticEaseRoutedEvent()
    {
        // Create a RoutedEventArgs instance.
        RoutedEventArgs routedEventArgs = new(routedEvent: BeginQuarticEaseEvent);

        // Raise the event, which will bubble up through the element tree.
        RaiseEvent(routedEventArgs);
    }
}
