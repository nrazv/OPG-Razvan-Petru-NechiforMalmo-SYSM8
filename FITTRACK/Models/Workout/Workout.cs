using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FITTRACK.Models;

public class Workout
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public WorkoutType WorkoutType { get; set; }
    public Duration TimeSpan { get; set; }
    public int CaloriesBurned { get; set; }
    public string Notes { get; set; } = string.Empty;

    virtual public int CalculateCaloriesBurned()
    {
        return 0;
    }
}
