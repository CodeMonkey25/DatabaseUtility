using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;

namespace DatabaseUtility.ViewModels;

public partial class ServersViewModel : ViewModelBase
{
    private readonly ISettingsService? _settingsService;
    private readonly IObservable<bool>? _isDirty;
    private readonly IObservable<bool>? _canAddServer;
    private readonly IObservable<bool>? _isServerSelected;
    private readonly Settings _settings = new();
    public override ViewTypes ViewType => ViewTypes.Servers;
    
    [Reactive] private ObservableCollectionExtended<string> _databaseServers = [];
    [Reactive] private string _databaseServer = string.Empty;
    [Reactive] private string? _selectedDatabaseServer;
    [Reactive] private int _selectedDatabaseServerIndex = -1;

    public ServersViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public ServersViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _settings = _settingsService.Get();
        
        _isDirty = Observable
            .FromEventPattern<NotifyCollectionChangedEventArgs>(DatabaseServers, "CollectionChanged")
            .Select(_ => AreCollectionsDifferent(DatabaseServers, _settings.DatabaseServers));
        _canAddServer = this.WhenAnyValue(vm => vm.DatabaseServer, server => !string.IsNullOrWhiteSpace(server));
        _isServerSelected = this.WhenAnyValue(vm => vm.SelectedDatabaseServer, server => !string.IsNullOrWhiteSpace(server));
        Cancel();
    }

    [ReactiveCommand(CanExecute = nameof(_canAddServer))]
    private void AddServer()
    {
        DatabaseServers.Add(DatabaseServer);
    }

    [ReactiveCommand(CanExecute = nameof(_isServerSelected))]
    private void RemoveServer()
    {
        DatabaseServers.Remove(SelectedDatabaseServer!);
    }

    [ReactiveCommand(CanExecute = nameof(_isServerSelected))]
    private void MoveServerUp()
    {
        int i = SelectedDatabaseServerIndex;
        if (i == 0) return;
        DatabaseServers.Move(i, i - 1);
        SelectedDatabaseServerIndex = i - 1;
    }

    [ReactiveCommand(CanExecute = nameof(_isServerSelected))]
    private void MoveServerDown()
    {
        int i = SelectedDatabaseServerIndex;
        if (i == DatabaseServers.Count - 1) return;
        DatabaseServers.Move(i, i + 1);
        SelectedDatabaseServerIndex = i + 1;
    }
    
    [ReactiveCommand(CanExecute = nameof(_isDirty))]
    private void Save()
    {
        _settings.DatabaseServers = _databaseServers.ToList();
        _settingsService?.Save(_settings);
        Cancel();
    }
    
    [ReactiveCommand(CanExecute = nameof(_isDirty))]
    private void Cancel()
    {
        _databaseServers.Clear();
        _databaseServers.Add(_settings.DatabaseServers);
    }

    private bool AreCollectionsDifferent(IList<string> first, IList<string> second)
    {
        if (first.Count != second.Count) return true;
        for (int i = 0; i < first.Count; i++)
        {
            if (!first[i].Equals(second[i])) return true;
        }
        return false;
    }
}