using Avalonia.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using SukiUI.Controls;
using SukiUI.Dialogs;
using YASAM.ViewModels;

namespace YASAM.Views;

public partial class MainWindow : SukiWindow
{
    public static ISukiDialogManager DialogManager = new SukiDialogManager();

    public MainWindow()
    {
        InitializeComponent();
        
        DialogHost.Manager = DialogManager;
    }
}