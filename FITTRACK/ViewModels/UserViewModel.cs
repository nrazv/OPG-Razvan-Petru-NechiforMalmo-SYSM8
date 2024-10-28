using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using FITTRACK.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace FITTRACK.ViewModels;
public class UserViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private InMemoryDataService _dataService;
    private INavigationService _navigationService;
    private User _authenticatedUser;
    private Workout _selectedWorkout;
    private bool _isWorkoutSelected = false;
    private bool _canSearch = false;

    private string _workoutTypeFilter;
    public string WorkoutTypeFilter
    {
        get => _workoutTypeFilter;
        set
        {
            _workoutTypeFilter = value;
            _canSearch = true;
            OnPropertyChanged();
        }
    }

    private DateTime? _workoutEndDateFilter = null;
    public DateTime? WorkoutEndDateFilter
    {
        get => _workoutEndDateFilter;
        set
        {
            _workoutEndDateFilter = value;
            OnPropertyChanged();
        }
    }
    private DateTime _workoutStartDateFilter = DateTime.Now;

    public DateTime WorkoutStartDateFilter
    {
        get => _workoutStartDateFilter;
        set
        {
            _workoutStartDateFilter = value;
            _canSearch = true;
            OnPropertyChanged();
        }
    }



    private string _userInfo;
    public List<string> WorkoutTypes { get; } = new() { WorkoutType.Cardio.ToString(), WorkoutType.Strength.ToString() };
    public string UserInfo { get => _userInfo; set { _userInfo = $"{value} workout's"; OnPropertyChanged(); } }

    public bool IsWorkoutSelected
    {
        get => _isWorkoutSelected;
        set
        {
            _isWorkoutSelected = value;
            OnPropertyChanged();
        }
    }

    public Workout SelectedWorkout
    {
        get => _selectedWorkout;
        set
        {
            _selectedWorkout = value;
            IsWorkoutSelected = true;
            if (value is null)
            {
                IsWorkoutSelected = false;
            }
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Workout> Workouts { get; set; }
    public RelayCommand NavigateToProfileCommand { get; set; }
    public RelayCommand OpenAddWorkoutWindowCommand { get; set; }
    public RelayCommand DeleteWorkoutCommand { get; set; }
    public RelayCommand ClearAllFiltersCommand { get; set; }
    public RelayCommand SearchCommand { get; set; }


    public User AuthenticatedUser
    {
        get => _authenticatedUser;
        set
        {
            _authenticatedUser = value;
            OnPropertyChanged();
        }
    }

    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            _navigationService = value;
            OnPropertyChanged();
        }
    }

    public UserViewModel(INavigationService navigationService, IDataService dataService)
    {
        NavigationService = navigationService;
        _dataService = (InMemoryDataService)dataService;
        setAuthenticatedUser();
        setWorkouts();
        createCommands();
    }

    private void openAddWorkoutWindow(object obj)
    {
        AddWorkoutWindowView addWorkoutWindow = new AddWorkoutWindowView(_dataService);
        addWorkoutWindow.Show();
    }

    private void deleteWorkout(object obj)
    {
        var id = SelectedWorkout.Id;
        string messageBoxText = "Are you sure you want to delete this workout ?";
        string caption = "Delete Workout";
        MessageBoxButton button = MessageBoxButton.YesNo;
        MessageBoxImage icon = MessageBoxImage.Warning;
        MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

        if (result is MessageBoxResult.Yes)
        {
            AuthenticatedUser.DeleteWorkout(SelectedWorkout.Id);
            Workouts.Remove(SelectedWorkout);
            SelectedWorkout = null;
        }
    }

    private void filterSearch(object obj)
    {
        search(WorkoutStartDateFilter, WorkoutEndDateFilter, WorkoutTypeFilter);
    }

    private void search(DateTime startDate, DateTime? endDate = null, string? workoutType = null)
    {

        Workouts.Clear();
        AuthenticatedUser.Workouts.Where(workout =>
                workout.Date.Date >= startDate && (!endDate.HasValue || workout.Date.Date <= endDate.Value) &&
                (workoutType == null || workout.WorkoutType.ToString() == workoutType)
                )
            .ToList()
            .ForEach(Workouts.Add);
    }


    private void clearAllFilters(object obj)
    {
        Workouts.Clear();
        AuthenticatedUser.Workouts.ToList().ForEach(Workouts.Add);
        WorkoutTypeFilter = null;
        _canSearch = false;
    }

    private void setWorkouts()
    {
        Workouts = new ObservableCollection<Workout>();
        AuthenticatedUser.Workouts.ToList().ForEach(Workouts.Add);
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (Workout newItem in e.NewItems)
                {
                    if (WorkoutTypeFilter is not null)
                    {
                        if (WorkoutTypeFilter == newItem.WorkoutType.ToString())
                        {
                            Workouts.Add(newItem);
                        }
                        return;
                    }

                    Workouts.Add(newItem);
                }
                break;
        }
    }

    private void setAuthenticatedUser()
    {
        AuthenticatedUser = _dataService.AuthenticatedUser;
        AuthenticatedUser.Workouts.CollectionChanged += OnCollectionChanged;
        UserInfo = AuthenticatedUser.UserName;
    }

    private void createCommands()
    {
        OpenAddWorkoutWindowCommand = new RelayCommand(execute: openAddWorkoutWindow, canExecute: e => true);
        DeleteWorkoutCommand = new RelayCommand(execute: deleteWorkout, canExecute: e => SelectedWorkout is not null);
        ClearAllFiltersCommand = new RelayCommand(execute: clearAllFilters, canExecute: e => true);
        SearchCommand = new RelayCommand(execute: filterSearch, canExecute: e => _canSearch);
    }


}

