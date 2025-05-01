using System;
using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Splat;

namespace DatabaseUtility.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingsService? _settingsService;
    private IObservable<bool>? _isDirty;
    private readonly Settings _settings = new();
    public override ViewTypes ViewType => ViewTypes.Settings;

    [Reactive] private bool _showOnlyRM12Databases;
    [Reactive] private bool _showOnlyDefaultLocations;
    [Reactive] private string _databaseNameFilter = string.Empty;
    [Reactive] private string _databaseUser = string.Empty;
    [Reactive] private string _databasePassword = string.Empty;

    public SettingsViewModel() { /* constructor for axaml designer */ }
    
    [DependencyInjectionConstructor]
    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        _settings = _settingsService.Get();
        
        _isDirty = this.WhenAnyValue(
            vm => vm.ShowOnlyRM12Databases,
            vm => vm.ShowOnlyDefaultLocations,
            vm => vm.DatabaseNameFilter,
            vm => vm.DatabaseUser,
            vm => vm.DatabasePassword,
            (showOnlyRM12Databases, showOnlyDefaultLocations, databaseNameFilter, databaseUser, databasePassword) =>
                showOnlyRM12Databases != _settings.ShowOnlyRM12Databases
                || showOnlyDefaultLocations != _settings.ShowOnlyDefaultLocations
                || databaseNameFilter != _settings.DatabaseNameFilter
                || databaseUser != _settings.DatabaseUser
                || databasePassword != _settings.DatabasePassword
            );

        Cancel();
    }
    
    [ReactiveCommand(CanExecute = nameof(_isDirty))]
    internal void Save()
    {
        _settings.ShowOnlyRM12Databases = ShowOnlyRM12Databases;
        _settings.ShowOnlyDefaultLocations = ShowOnlyDefaultLocations;
        _settings.DatabaseNameFilter = DatabaseNameFilter;
        _settings.DatabaseUser = DatabaseUser;
        _settings.DatabasePassword = DatabasePassword;
        _settingsService?.Save(_settings);
    }
    
    [ReactiveCommand(CanExecute = nameof(_isDirty))]
    internal void Cancel()
    {
        ShowOnlyRM12Databases = _settings.ShowOnlyRM12Databases;
        ShowOnlyDefaultLocations = _settings.ShowOnlyDefaultLocations;
        DatabaseNameFilter = _settings.DatabaseNameFilter;
        DatabaseUser = _settings.DatabaseUser;
        DatabasePassword = _settings.DatabasePassword;
    }
}