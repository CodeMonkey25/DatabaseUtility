using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DatabaseUtility.Views;

public partial class DataInfoFileView : UserControl
{
    public DataInfoFileView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}