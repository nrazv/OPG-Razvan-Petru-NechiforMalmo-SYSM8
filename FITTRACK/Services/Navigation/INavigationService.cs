using FITTRACK.MVVM;


namespace FITTRACK.Services.Navigation;

public interface INavigationService
{
    ViewModelBase CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;

}