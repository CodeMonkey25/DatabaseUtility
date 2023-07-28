using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DatabaseUtility.Models;

public class DatabaseConnection : INotifyPropertyChanged, ICloneable
{
    private string _name = string.Empty;
    private string _server = string.Empty;
    private string _userName = string.Empty;
    private string _password = string.Empty;

    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
    }

    public string Server
    {
        get => _server;
        set => SetField(ref _server, value);
    }

    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    #endregion

    #region ICloneable
    public object Clone()
    {
        return new DatabaseConnection().OverwriteWith(this);
    }
    #endregion

    public DatabaseConnection OverwriteWith(DatabaseConnection that)
    {
        this.Name = that.Name;
        this.Server = that.Server;
        this.UserName = that.UserName;
        this.Password = that.Password;
        return this;
    }
}