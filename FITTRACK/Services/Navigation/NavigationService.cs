using FITTRACK.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.Services.Navigation;

public class NavigationService : NotifyChanges, INavigationService
{
    private ViewModelBase _currentView;

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
    private Func<Type, ViewModelBase> _viewModelFactory { get; }
    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<ViewType>() where ViewType : ViewModelBase
    {
        CurrentView = _viewModelFactory.Invoke(typeof(ViewType));
    }
}
