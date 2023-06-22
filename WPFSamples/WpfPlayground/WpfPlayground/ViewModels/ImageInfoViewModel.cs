using JMWToolkit.MVVM.ViewModels;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using WpfPlayground.Interfaces;

namespace WpfPlayground.ViewModels;

public class ImageInfoViewModel : ViewModelBase, IImageInfoViewModel
{
    public ImageInfoViewModel(string description, MessageBoxImage image)
    {
        Description = description;
        Image = image;
        if (image == MessageBoxImage.Warning)
        {
            ImageSource = SystemIcons.Warning.ToImageSource();
        }
        else if (image == MessageBoxImage.Error)
        {
            ImageSource = SystemIcons.Error.ToImageSource();
        }
        else if (image == MessageBoxImage.Question)
        {
            ImageSource = SystemIcons.Question.ToImageSource();
        }
        else
        {
            ImageSource = SystemIcons.Information.ToImageSource();
        }

    }

    public MessageBoxImage Image { get; private set; }
    public ImageSource ImageSource { get; private set; }
    public string Description { get; private set; }
}
