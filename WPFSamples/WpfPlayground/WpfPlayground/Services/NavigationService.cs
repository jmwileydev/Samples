using CommunityToolkit.Mvvm.ComponentModel;
using System;
using WpfPlayground.Interfaces;

namespace WpfPlayground.Services;

public class NavigationService : ObservableObject, INavigationService
{
    private Func<Type, IViewModel> _viewModelFactory;
    public IViewModel _currentView;
    
    public IViewModel CurrentView
    {
        get => _currentView;

        private set
        { 
            if (_currentView != value)
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
    }

    public NavigationService(Func<Type, IViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : IViewModel
    {
        IViewModel viewModel =  _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}
