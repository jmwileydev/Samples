using JMWToolkit.MVVM.ViewModels;
using System;
using System.Windows;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

public class AddNewItemViewModel : ViewModelBase, IAddNewItemViewModel
{
    public string NewItem { get; set; } = String.Empty;
}
