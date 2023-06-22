using System.Windows;
using System.Windows.Media;

namespace WpfPlayground.Interfaces
{
    public interface IImageInfoViewModel
    {
        string Description { get; }
        MessageBoxImage Image { get; }
        ImageSource ImageSource { get; }
    }
}