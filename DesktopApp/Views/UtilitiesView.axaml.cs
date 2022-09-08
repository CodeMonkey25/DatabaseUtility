using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopApp.ViewModels;

namespace DesktopApp.Views
{
    public partial class UtilitiesView : ReactiveUserControl<UtilitiesViewModel>
    {
        public UtilitiesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}