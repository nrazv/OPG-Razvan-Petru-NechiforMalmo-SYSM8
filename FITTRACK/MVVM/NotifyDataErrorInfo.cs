using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FITTRACK.MVVM;

public class NotifyDataErrorInfo : INotifyDataErrorInfo
{
    public Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();
    public bool HasErrors => Errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName is null) return Enumerable.Empty<string>();

        if (Errors.ContainsKey(propertyName))
        {
            return Errors[propertyName];
        }
        else
        {
            return Enumerable.Empty<string>();
        }
    }

    public void Validate(string propertyName, object propertyValue)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateProperty(propertyValue, new ValidationContext(this) { MemberName = propertyName }, results);

        if (results.Any())
        {
            var errors = new List<string>();
            results.ForEach(x => errors.Add(x.ErrorMessage ?? ""));

            if (Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = errors;
            }

            if (!Errors.ContainsKey(propertyName))
            {

                Errors.Add(propertyName, errors);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        else
        {
            Errors.Remove(propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    public void NotifyErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}