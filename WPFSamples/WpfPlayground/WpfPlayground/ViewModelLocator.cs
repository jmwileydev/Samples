﻿using Microsoft.Extensions.DependencyInjection;
using WpfPlayground.Interfaces;

namespace WpfPlayground;

// Helper class to locate the MainWindowViewModel for my XAML.
public class ViewModelLocator
{
    public IMainWindowViewModel MainWindowViewModel => App.AppHost.Services.GetRequiredService<IMainWindowViewModel>();

    public IMessageBoxDemoAreaViewModel MessageBoxDemoAreaViewModel =>
        App.AppHost.Services.GetRequiredService<IMessageBoxDemoAreaViewModel>();
}
