using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FITTRACK.ViewModels;

public class AddWorkoutWindowViewMode : ViewModelBase
{
    private InMemoryDataService _dataService;
    private DateTime _date = DateTime.Now;
    private Duration _timeSpan;
    private int _caloriesBurned;
    private int _minutes = 00;
    private int _seconds = 00;
    private int _hours = 00;
    private string _notes = string.Empty;
    private WorkoutType _workoutType;

    public RelayCommand AddWorkoutCommand { get; set; }

    public ObservableCollection<WorkoutType> WorkoutTypes { get; set; }

    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }

    public int Minutes { get => _minutes; set { _minutes = value; OnPropertyChanged(); } }
    public int Hours { get => _hours; set { _hours = value; OnPropertyChanged(); } }
    public int Seconds { get => _seconds; set { _seconds = value; OnPropertyChanged(); } }
    public Duration TimeSpan { get => _timeSpan; set { _timeSpan = value; OnPropertyChanged(); } }
    public int CaloriesBurned { get => _caloriesBurned; set { _caloriesBurned = value; OnPropertyChanged(); } }
    public string Notes { get => _notes; set { _notes = value; OnPropertyChanged(); } }
    public WorkoutType WorkoutType { get => _workoutType; set { _workoutType = value; OnPropertyChanged(); } }


    public AddWorkoutWindowViewMode(IDataService dataService)
    {
        WorkoutTypes = new ObservableCollection<WorkoutType> { WorkoutType.Cardio, WorkoutType.Strength };
        AddWorkoutCommand = new RelayCommand(execute: addNewWorkout, canExecute: e => true);
        _dataService = (InMemoryDataService)dataService;
    }



    private void addNewWorkout(object obj)
    {

        Workout workout = new Workout();
        workout.Id = Guid.NewGuid();
        workout.WorkoutType = WorkoutType;
        workout.Date = Date;
        workout.Notes = Notes;
        workout.CaloriesBurned = CaloriesBurned;
        workout.TimeSpan = new TimeSpan(hours: Hours, minutes: Minutes, seconds: Seconds);

        _dataService.AuthenticatedUser.AddWorkout(workout);

        Window addWorkoutWindow = (AddWorkoutWindowView)obj;
        addWorkoutWindow.Close();
    }
}
