using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Services.Navigation;
using FITTRACK.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace FITTRACK.ViewModels;

public class WorkoutsViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private InMemoryDataService _dataService;
    private INavigationService _navigationService;
    private User _authenticatedUser;
    private Workout _selectedWorkout;
    private bool _isWorkoutSelected = false;
    private bool _canSearch = false;
    private string _workoutTypeFilter;
    private bool _workoutEditOn = false;
    private Visibility _editWorkoutVisibility = Visibility.Visible;
    private DateTime? _workoutEndDateFilter = null;
    private DateTime _workoutStartDateFilter = DateTime.Now;
    private string _userInfo;


    public List<string> WorkoutTypes { get; } = new() { WorkoutType.Cardio.ToString(), WorkoutType.Strength.ToString() };
    public ObservableCollection<Workout> Workouts { get; set; }

    public RelayCommand OpenAddWorkoutWindowCommand { get; set; }
    public RelayCommand DeleteWorkoutCommand { get; set; }
    public RelayCommand ClearAllFiltersCommand { get; set; }
    public RelayCommand SearchCommand { get; set; }
    public RelayCommand ToggleEditWorkout { get; set; }
    public RelayCommand SaveWorkoutChangesCommand { get; set; }
    public RelayCommand CopyAsTemplateCommand { get; set; }





    public bool WorkoutEditOn
    {
        get => _workoutEditOn;
        set { _workoutEditOn = value; OnPropertyChanged(); }
    }

    public Visibility EditWorkoutVisibility
    {
        get => _editWorkoutVisibility;
        set { _editWorkoutVisibility = value; OnPropertyChanged(); }
    }

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

    public DateTime? WorkoutEndDateFilter
    {
        get => _workoutEndDateFilter;
        set
        {
            _workoutEndDateFilter = value;
            OnPropertyChanged();
        }
    }

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


    public string UserInfo
    {
        get => _userInfo;
        set { _userInfo = $"{value} workout's"; OnPropertyChanged(); }
    }

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




    public WorkoutsViewModel(INavigationService navigationService, IDataService dataService)
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

    private void toggleEditWorkoutVisibility(Object obj)
    {
        WorkoutEditOn = !WorkoutEditOn;
        EditWorkoutVisibility = WorkoutEditOn == true ? Visibility.Collapsed : Visibility.Visible;
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
            AuthenticatedUser.Workouts.Remove(SelectedWorkout);
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
        WorkoutEditOn = false;
        EditWorkoutVisibility = Visibility.Visible;
    }

    private void setWorkouts()
    {
        Workouts = new ObservableCollection<Workout>();
        AuthenticatedUser.Workouts.ToList().ForEach(Workouts.Add);
        OnPropertyChanged(nameof(Workouts));
    }

    private void OnWorkoutsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {

        // This makes a sync of deleted and added objects
        // The filter creates a new list and does not use the original AuthenticatedUser list of items and this makes them out of sync
        // When I'm filtering data and I add or delete an object this need's to be reflected on the UI 
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

    // needed to clear data when a new user logs in 
    public void UpdateData()
    {
        setAuthenticatedUser();
        setWorkouts();
    }


    // save changes made to workout
    private void saveWorkoutChanges(object obj)
    {
        var workoutToUpdate = AuthenticatedUser.Workouts.
               Where(w => w.Id == SelectedWorkout.Id).First();
        workoutToUpdate.Notes = SelectedWorkout.Notes;
        workoutToUpdate.Date = SelectedWorkout.Date;
        workoutToUpdate.WorkoutType = SelectedWorkout.WorkoutType;
        workoutToUpdate.CaloriesBurned = SelectedWorkout.CaloriesBurned;

        WorkoutEditOn = false;
        EditWorkoutVisibility = Visibility.Visible;
    }

    // set data for the current authenticated user
    private void setAuthenticatedUser()
    {
        AuthenticatedUser = _dataService.AuthenticatedUser;
        AuthenticatedUser.Workouts.CollectionChanged += OnWorkoutsCollectionChanged;
        UserInfo = AuthenticatedUser.UserName;
    }

    // sets the workout as a template for "för att kunna kopiera ett träningspass som mall" 
    private void setWorkoutTemplate(object obj)
    {
        AuthenticatedUser.WorkoutAsTemplate = SelectedWorkout;
    }
    private void createCommands()
    {
        OpenAddWorkoutWindowCommand = new RelayCommand(execute: openAddWorkoutWindow, canExecute: e => true);
        DeleteWorkoutCommand = new RelayCommand(execute: deleteWorkout, canExecute: e => SelectedWorkout is not null);
        ClearAllFiltersCommand = new RelayCommand(execute: clearAllFilters, canExecute: e => true);
        SearchCommand = new RelayCommand(execute: filterSearch, canExecute: e => _canSearch);
        ToggleEditWorkout = new RelayCommand(execute: toggleEditWorkoutVisibility, canExecute: e => SelectedWorkout is not null);
        SaveWorkoutChangesCommand = new RelayCommand(execute: saveWorkoutChanges, canExecute: e => SelectedWorkout is not null);
        CopyAsTemplateCommand = new RelayCommand(execute: setWorkoutTemplate, canExecute: e => SelectedWorkout is not null);
    }
}
