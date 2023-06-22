using System.Collections.ObjectModel;

namespace WpfPlayground.Interfaces;

public interface IMessageBoxDemoAreaViewModel : IViewModel
{
    ObservableCollection<IImageInfoViewModel> ImageInfos { get; }
    IImageInfoViewModel SelectedItem { get; set; }
}