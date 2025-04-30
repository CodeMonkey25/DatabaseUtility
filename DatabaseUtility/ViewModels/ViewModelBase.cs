using System.Reactive.Disposables;
using DatabaseUtility.Utility;
using ReactiveUI;

namespace DatabaseUtility.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel 
{
    public ViewModelActivator Activator { get; } = new();

    public abstract ViewTypes ViewType { get; }

    public ViewModelBase()
    {
        this.WhenActivated(disposables => 
        {        
            HandleActivation(disposables);
            Disposable.Create(HandleDeactivation).DisposeWith(disposables);
        });
    }

    protected virtual void HandleActivation(CompositeDisposable disposables) { }
    protected virtual void HandleDeactivation() { }
}