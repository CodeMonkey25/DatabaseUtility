using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DesktopApp.ViewModels;
using Splat;

namespace DesktopApp.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>, IEnableLogger
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}