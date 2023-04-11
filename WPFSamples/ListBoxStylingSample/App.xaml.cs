﻿using ListBoxStylingSample.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace ListBoxStylingSample;

public partial class App : Application
{
    public static IHost AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder().ConfigureServices(
            (hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<ITransactionListViewModel, TransactionListViewModel>();
            }).Build();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<MainWindow>().AddSingleton<ITransactionListViewModel, TransactionListViewModel>();
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
