using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace YASAM.Controls;

public partial class GameCard : UserControl
{
    public static readonly RoutedEvent<RoutedEventArgs> IdleEvent =
        RoutedEvent.Register<Control, RoutedEventArgs>(
            nameof(Idle),
            RoutingStrategies.Direct);

    public static readonly RoutedEvent<RoutedEventArgs> ShowAchievementsEvent =
        RoutedEvent.Register<Control, RoutedEventArgs>(
            nameof(ShowAchievements),
            RoutingStrategies.Direct);

    public static readonly RoutedEvent<RoutedEventArgs> ShowInSteamEvent =
        RoutedEvent.Register<Control, RoutedEventArgs>(
            nameof(ShowInSteam),
            RoutingStrategies.Direct);


    public static readonly StyledProperty<bool> DisplayIdleProperty =
        AvaloniaProperty.Register<GameCard, bool>(nameof(DisplayIdle), defaultValue: false);

    public bool DisplayIdle
    {
        get => GetValue(DisplayIdleProperty);
        set => SetValue(DisplayIdleProperty, value);
    }

    public static readonly StyledProperty<bool> DisplayStopProperty =
        AvaloniaProperty.Register<GameCard, bool>(nameof(DisplayStop), defaultValue: false);

    public bool DisplayStop
    {
        get => GetValue(DisplayStopProperty);
        set => SetValue(DisplayStopProperty, value);
    }


    public static readonly StyledProperty<bool> DisplayAchievementsProperty =
        AvaloniaProperty.Register<GameCard, bool>(nameof(DisplayAchievements), defaultValue: false);

    public bool DisplayAchievements
    {
        get => GetValue(DisplayAchievementsProperty);
        set => SetValue(DisplayAchievementsProperty, value);
    }

    public static readonly StyledProperty<bool> DisplaySteamStoreProperty =
        AvaloniaProperty.Register<GameCard, bool>(nameof(DisplaySteamStore), defaultValue: false);

    public bool DisplaySteamStore
    {
        get => GetValue(DisplaySteamStoreProperty);
        set => SetValue(DisplaySteamStoreProperty, value);
    }


    public GameCard()
    {
        InitializeComponent();
    }

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

    public event EventHandler<RoutedEventArgs> ShowInSteam
    {
        add => AddHandler(ShowInSteamEvent, value);
        remove => RemoveHandler(ShowInSteamEvent, value);
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

    private void ShowInSteamClicked(object? sender, RoutedEventArgs e)
    {
        var args = new RoutedEventArgs(ShowInSteamEvent);
        RaiseEvent(args);
    }
}