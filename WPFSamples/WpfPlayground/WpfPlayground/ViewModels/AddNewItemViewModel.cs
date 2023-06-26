using JMWToolkit.MVVM.ViewModels;
using System;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

/// <summary>
/// Simple ViewModel for the Add New Item dialog. Simply stores the new String
/// </summary>
public class AddNewItemViewModel : ViewModelBase, IAddNewItemViewModel
{
    public string NewItem { get; set; } = String.Empty;
}
