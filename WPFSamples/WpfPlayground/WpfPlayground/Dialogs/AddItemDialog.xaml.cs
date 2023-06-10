using JMWToolkit.MVVM.Helpers;
using System;
using System.Windows;

namespace WpfPlayground.Dialogs;

/// <summary>
/// Interaction logic for AddItemDialog.xaml
/// </summary>
public partial class AddItemDialog : Window
{
    public AddItemDialog()
    {
        InitializeComponent();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        IconHelper.RemoveIcon(this);
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }
}
