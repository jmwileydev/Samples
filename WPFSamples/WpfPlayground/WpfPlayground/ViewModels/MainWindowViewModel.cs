using CommunityToolkit.Mvvm.Input;
using JMWToolkit.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfPlayground.Dialogs;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private string _selectedItem = String.Empty;

    public ObservableCollection<string> Items { get; } = new()
    {
        "Item 1",
        "Item 2",
        "Item 3",
        "Item 4"
    };


    public string SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
    }

    public MainWindowViewModel()
    {
        AddNewItemCommand = new RelayCommand<Object>((_) =>
        {
            var dialogViewModel = App.AppHost.Services.GetRequiredService<IAddNewItemViewModel>();
            AddItemDialog dialog = new()
            {
                DataContext = dialogViewModel
            };

            if (dialog.ShowDialog() == true)
            {
                Items.Add(dialogViewModel.NewItem);
                SelectedItem = dialogViewModel.NewItem;
            }
        });
    }

    public ICommand AddNewItemCommand { get; private set; }

}
