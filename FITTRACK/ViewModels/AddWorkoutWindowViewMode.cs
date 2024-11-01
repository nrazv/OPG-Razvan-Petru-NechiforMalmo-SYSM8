using FITTRACK.Models;
using FITTRACK.MVVM;
using FITTRACK.Services.DataService;
using FITTRACK.Views;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Windows;

namespace FITTRACK.ViewModels;

public class AddWorkoutWindowViewMode : ViewModelBase
{
    private IDataService _dataService;
    private DateTime _date = DateTime.Now;
    private Duration _timeSpan;
    private int _caloriesBurned;
    private int _minutes = 00;
    private int _seconds = 00;
    private int _hours = 00;
    private string _notes = string.Empty;
    private WorkoutType _workoutType;


    public RelayCommand AddWorkoutCommand { get; set; }
    public RelayCommand PastTemplateCommand { get; set; }


    public ObservableCollection<WorkoutType> WorkoutTypes { get; set; }

    [Required]
    public DateTime Date
    {
        get => _date;
        set
        {
            _date = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public int Minutes
    {
        get => _minutes;
        set
        {
            _minutes = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public int Hours
    {
        get => _hours;
        set
        {
            _hours = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public int Seconds
    {
        get => _seconds;
        set
        {
            _seconds = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public Duration TimeSpan { get => _timeSpan; set { _timeSpan = value; OnPropertyChanged(); } }

    [Required]
    [Range(1, 99999)]
    public int CaloriesBurned
    {
        get => _caloriesBurned;
        set
        {
            _caloriesBurned = value;
            OnPropertyChanged();
        }
    }

    [Required]
    [MinLength(5)]
    public string Notes { get => _notes; set { _notes = value; OnPropertyChanged(); } }

    [Required]
    public WorkoutType WorkoutType { get => _workoutType; set { _workoutType = value; OnPropertyChanged(); } }


    public AddWorkoutWindowViewMode(IDataService dataService)
    {
        WorkoutTypes = new ObservableCollection<WorkoutType> { WorkoutType.Cardio, WorkoutType.Strength };
        AddWorkoutCommand = new RelayCommand(execute: addNewWorkout, canExecute: e => true);
        _dataService = dataService;
        PastTemplateCommand = new RelayCommand(execute: setWorkoutTemplate, canExecute: e =>
        {
            if (_dataService.GetAuthenticatedUser() is not null)
            {
                return _dataService.GetAuthenticatedUser().WorkoutAsTemplate is not null;
            }

            return false;
        });
    }

    private void setWorkoutTemplate(object obj)
    {
        var workoutTemplate = _dataService.GetAuthenticatedUser().WorkoutAsTemplate;
        Date = workoutTemplate.Date.Date;
        Notes = workoutTemplate.Notes;
        CaloriesBurned = workoutTemplate.CaloriesBurned;
        Minutes = workoutTemplate.TimeSpan.TimeSpan.Minutes;
        Hours = workoutTemplate.TimeSpan.TimeSpan.Hours;
        Seconds = workoutTemplate.TimeSpan.TimeSpan.Seconds;
        WorkoutType = workoutTemplate.WorkoutType;
    }


    private void addNewWorkout(object obj)
    {
        validateWorkout();

        if (Errors.Any())
        {
            return;
        }

        Workout workout = new Workout();
        workout.Id = Guid.NewGuid();
        workout.WorkoutType = WorkoutType;
        workout.Date = Date;
        workout.Notes = Notes;
        workout.CaloriesBurned = CaloriesBurned;
        workout.TimeSpan = new TimeSpan(hours: Hours, minutes: Minutes, seconds: Seconds);

        _dataService.GetAuthenticatedUser().AddWorkout(workout);

        Window addWorkoutWindow = (AddWorkoutWindowView)obj;
        addWorkoutWindow.Close();
    }


    private void validateWorkout()
    {
        Validate(nameof(CaloriesBurned), CaloriesBurned);
        Validate(nameof(Date), Date);
        Validate(nameof(Date), Date);
        Validate(nameof(Hours), Hours);
        Validate(nameof(Minutes), Minutes);
        Validate(nameof(Seconds), Seconds);
        Validate(nameof(WorkoutType), WorkoutType);
    }

}
