using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using WpfPlayground.ViewModels;

namespace WpfPlayground.Interfaces;

public record ImageInfo
{
    public ImageInfo(string description, MessageBoxImage image)
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

public interface IMessageBoxDemoAreaViewModel
{
    ObservableCollection<ImageInfo> ImageInfos { get; }
    ImageInfo SelectedItem { get; set; }
}