using JMWToolkit.MVVM.ViewModels;
using System;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

public class AddNewItemViewModel : ViewModelBase, IAddNewItemViewModel
{
    public string NewItem { get; set; } = String.Empty;
}
