namespace WpfPlayground.Interfaces;

public interface INavigationService
{
    IViewModel CurrentView { get; }

    void NavigateTo<T>() where T : IViewModel;
}