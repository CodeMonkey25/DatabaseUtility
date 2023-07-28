using ReactiveUI;

namespace DatabaseUtility.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _mySubViewModel = new DataInfoFileViewModel();
    public ViewModelBase MySubViewModel
    {
        get => _mySubViewModel;
        set => this.RaiseAndSetIfChanged(ref _mySubViewModel, value);
    }
}