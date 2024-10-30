
using NSwag.Collections;
using System.Collections.ObjectModel;

namespace FITTRACK.Models;
public class User : Person
{
    private ObservableCollection<Workout> _workouts;

    public ObservableCollection<Workout> Workouts { get => _workouts; }

    public string Country { get; set; } = string.Empty;
    public UserSecurityQuestion SecurityQuestion { get; set; }

    public Workout? WorkoutAsTemplate { get; set; }

    public User()
    {
        _workouts = new ObservableCollection<Workout>();
    }

    public void AddWorkout(Workout workout)
    {
        _workouts.Add(workout);
    }

    public void DeleteWorkout(Guid id)
    {
    }
}
