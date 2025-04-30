using System;
using System.Reactive.Linq;
using DatabaseUtility.Utility;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;

namespace DatabaseUtility.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public override ViewTypes ViewType => ViewTypes.Main;
    
    private IViewFactory? ViewFactory { get; }
    private readonly IObservable<bool>? _canSwitchToDataInfoExecute;
    private readonly IObservable<bool>? _canSwitchToSettingsExecute;
    private readonly IObservable<bool>? _canSwitchToServersExecute;

    [Reactive]
    private ViewModelBase? _contentViewModel;

    public MainWindowViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public MainWindowViewModel(IViewFactory viewFactory)
    {
        ViewFactory = viewFactory;
        
        _canSwitchToDataInfoExecute = this.WhenAnyValue(vm => vm.ContentViewModel).WhereNotNull().Select(cvm => cvm.ViewType != ViewTypes.DataInfo);
        _canSwitchToSettingsExecute = this.WhenAnyValue(vm => vm.ContentViewModel).WhereNotNull().Select(cvm => cvm.ViewType != ViewTypes.Settings);
        _canSwitchToServersExecute = this.WhenAnyValue(vm => vm.ContentViewModel).WhereNotNull().Select(cvm => cvm.ViewType != ViewTypes.Servers);
        
        SwitchToDataInfo();
    }

    [ReactiveCommand(CanExecute = nameof(_canSwitchToDataInfoExecute))]
    private void SwitchToDataInfo()
    {
        ContentViewModel = ViewFactory?.CreateView(ViewTypes.DataInfo) ?? throw new Exception("unable to locate DataInfoView");
    }
    
    [ReactiveCommand(CanExecute = nameof(_canSwitchToSettingsExecute))]
    internal void SwitchToSettings()
    {
        ContentViewModel = ViewFactory?.CreateView(ViewTypes.Settings) ?? throw new Exception("unable to locate SettingsView");
    }
    
    [ReactiveCommand(CanExecute = nameof(_canSwitchToServersExecute))]
    internal void SwitchToServers()
    {
        ContentViewModel = ViewFactory?.CreateView(ViewTypes.Servers) ?? throw new Exception("unable to locate ServersView");
    }
}