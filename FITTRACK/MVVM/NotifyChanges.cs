using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace FITTRACK.MVVM;

public class NotifyChanges : NotifyDataErrorInfo, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
