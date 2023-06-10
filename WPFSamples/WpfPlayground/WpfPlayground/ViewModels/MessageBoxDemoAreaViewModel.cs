using JMWToolkit.MVVM.Helpers;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfPlayground.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System;
using JMWToolkit.MVVM.ViewModels;
using System.Windows.Input;
using JMWToolkit.MVVM.Dialogs;
using System.Windows.Controls;

namespace WpfPlayground.ViewModels;

public static class IconExtensions
{
    public static ImageSource ToImageSource(this Icon icon)
    {
        return Imaging.CreateBitmapSourceFromHIcon(
            icon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
    }
}

[ValueConversion(typeof(String), typeof(List<Inline>))]
public class ButtonCheckBoxConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        String buttonText = (String)value;
        return StringResourceHelpers.LoadAndFormatStringResource("MessageBoxButtonCheckBoxFormat", buttonText);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class MessageBoxDemoAreaViewModel : ViewModelBase, IMessageBoxDemoAreaViewModel
{
    private string _caption = StringResourceHelpers.LoadStringResource("RichText");
    private string _title = StringResourceHelpers.LoadStringResource("DefaultTitle");
    private string _status = StringResourceHelpers.LoadStringResource("InitialStatusPrompt");
    private bool _showingResult = false;

    public MessageBoxDemoAreaViewModel()
    {
        SelectedItem = ImageInfos[0];

        ShowCommand = CreateRelayCommand(
            () =>
            {
                var result = MessageBoxEx.ShowDialog(
                    Caption,
                    Title,
                    MessageBoxButton,
                    SelectedItem.Image);
                switch(result)
                {
                    case MessageBoxResult.OK:
                        Status = StringResourceHelpers.LoadStringResource("OkButtonResult");
                        break;

                    case MessageBoxResult.Cancel:
                        Status = StringResourceHelpers.LoadStringResource("CancelButtonResult");
                        break;

                    case MessageBoxResult.Yes:
                        Status = StringResourceHelpers.LoadStringResource("YesButtonResult");
                        break;

                    case MessageBoxResult.No:
                        Status = StringResourceHelpers.LoadStringResource("NoButtonResult");
                        break;
                }

                _showingResult = true;
            },
            () => { return Caption.Length != 0 && Title.Length != 0; },
            "Caption", "Title");

        RadioButtonCheckedCommand = CreateRelayCommand<MessageBoxButton>(
            (b) =>
            {
                MessageBoxButton = b;
            },
            (b) =>
            {
                return b != MessageBoxButton;
            },
            "MessageBoxButton");
    }

    public ObservableCollection<ImageInfo> ImageInfos { get; private set; } = new()
    {
        new ImageInfo(
            StringResourceHelpers.LoadStringResource("MessageBoxImage_Information"),
            MessageBoxImage.Information),
        new ImageInfo(
            StringResourceHelpers.LoadStringResource("MessageBoxImage_Warning"),
            MessageBoxImage.Warning),
        new ImageInfo(
            StringResourceHelpers.LoadStringResource("MessageBoxImage_Error"),
            MessageBoxImage.Error),
        new ImageInfo(
            StringResourceHelpers.LoadStringResource("MessageBoxImage_Question"),
            MessageBoxImage.Question)
    };

    public string Title
    {
        get => _title;
        set
        {
            if (value != _title)
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }
    }

    public string Caption
    {
        get => _caption;
        set
        {
            if (value != _caption)
            {
                _caption = value;
                OnPropertyChanged();
            }
        }
    }

    public ImageInfo SelectedItem { get; set; }

    public bool OkButtonChecked
    {
        get => MessageBoxButton == MessageBoxButton.OK;
    }

    public bool OkCancelButtonChecked
    {
        get => MessageBoxButton == MessageBoxButton.OKCancel;
    }

    public bool YesNoButtonChecked
    {
        get => MessageBoxButton == MessageBoxButton.YesNo;
    }

    public bool YesNoCancelButtonChecked
    {
        get => MessageBoxButton == MessageBoxButton.YesNoCancel;
    }

    public ICommand RadioButtonCheckedCommand { get; private set; }

    public ICommand ShowCommand { get; private set; }

    private MessageBoxButton _messageBoxButton;

    public MessageBoxButton MessageBoxButton
    {
        get => _messageBoxButton;
        private set
        {
            if (value != _messageBoxButton)
            {
                _messageBoxButton = value;
                OnPropertyChanged();
            }
        }
    }

    public string Status
    {
        get => _status;
        private set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
            }
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName != "Status" && !_showingResult)
        {
            // reset the status once they change a property
            Status = StringResourceHelpers.LoadStringResource("InitialStatusPrompt");
        }
    }
}
