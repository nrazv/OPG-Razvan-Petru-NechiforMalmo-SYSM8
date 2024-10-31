using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.Models;

class AdminWorkout : Workout
{
    public string WorkoutOwner { get; set; }
}
