using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FITTRACK.ViewModels;

class AdminWorkoutsViewModel : ViewModelBase
{
    IDataService _dataService;

    private AdminWorkout _selectedWorkout;
    public ObservableCollection<AdminWorkout> Workouts { get; set; }
    public RelayCommand DeleteWorkoutCommand { get; set; }

    public AdminWorkout SelectedWorkout
    {
        get => _selectedWorkout;
        set
        {
            _selectedWorkout = value;
            OnPropertyChanged();
        }
    }

    public AdminWorkoutsViewModel(IDataService dataService)
    {
        _dataService = dataService;
        setWorkouts();

        DeleteWorkoutCommand = new RelayCommand(execute: deleteWorkout, canExecute: e => SelectedWorkout is not null);
    }

    private void setWorkouts()
    {
        Workouts = new ObservableCollection<AdminWorkout>();
        foreach (User user in _dataService.AllUsers())
        {
            if (user is Admin)
            {
                continue;
            }

            foreach (Workout work in user.Workouts)
            {
                AdminWorkout adminWorkout = new AdminWorkout();
                adminWorkout.WorkoutType = work.WorkoutType;
                adminWorkout.WorkoutOwnerId = user.Id;
                adminWorkout.CaloriesBurned = work.CaloriesBurned;
                adminWorkout.Date = work.Date;
                adminWorkout.Id = work.Id;
                adminWorkout.Notes = work.Notes;
                adminWorkout.TimeSpan = work.TimeSpan;
                Workouts.Add(adminWorkout);
            }
        }
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
            _dataService.DeleteUserWorkout(SelectedWorkout.Id, SelectedWorkout.WorkoutOwnerId);
            Workouts.Remove(SelectedWorkout);
            SelectedWorkout = null;
        }
    }
}
