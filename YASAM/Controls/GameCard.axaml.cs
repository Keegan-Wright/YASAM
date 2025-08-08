using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace YASAM.Controls;

public partial class GameCard : UserControl
{
    public static readonly RoutedEvent<RoutedEventArgs> IdleEvent =
        RoutedEvent.Register<Control, RoutedEventArgs>(
            nameof(Idle),
            RoutingStrategies.Direct);
    
    public static readonly RoutedEvent<RoutedEventArgs> ShowAchievementsEvent =
        RoutedEvent.Register<Control, RoutedEventArgs>(
            nameof(Idle),
            RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> Idle
    { 
        add => AddHandler(IdleEvent, value);
        remove => RemoveHandler(IdleEvent, value);
    }
    
    public event EventHandler<RoutedEventArgs> ShowAchievements
    { 
        add => AddHandler(ShowAchievementsEvent, value);
        remove => RemoveHandler(ShowAchievementsEvent, value);
    }

    
    public GameCard()
    {
        InitializeComponent();
    }

    private void IdleGameClicked(object? sender, RoutedEventArgs e)
    {
        var args = new RoutedEventArgs(IdleEvent);
        RaiseEvent(args);
    }

    private void ShowAchivementsClicked(object? sender, RoutedEventArgs e)
    {
        var args = new RoutedEventArgs(ShowAchievementsEvent);
        RaiseEvent(args);
    }
}