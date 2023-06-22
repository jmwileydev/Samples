using JMWToolkit.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using WpfPlayground.Interfaces;
using WpfPlayground.Services;
using WpfPlayground.ViewModels;

namespace WpfPlayground;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IHost AppHost { get; private set; }

    public App()
    {
        AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
        AppHost = Host.CreateDefaultBuilder().ConfigureServices(
            (services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
                services.AddSingleton<IAddNewItemViewModel, AddNewItemViewModel>();
                services.AddSingleton<IMessageBoxDemoAreaViewModel, MessageBoxDemoAreaViewModel>();
                services.AddSingleton<IAnimationDemoViewModel, AnimationDemoViewModel>();
                services.AddSingleton<IComboBoxWithAddDemoViewModel, ComboBoxWithAddDemoViewModel>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<Func<Type, IViewModel>>(
                    serviceProvider => viewModelType => (IViewModel)serviceProvider.GetRequiredService(viewModelType));
            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost.StartAsync();
        var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost.StopAsync();
        base.OnExit(e);
    }
}
