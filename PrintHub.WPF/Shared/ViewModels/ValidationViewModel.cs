using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FluentValidation;
using FluentValidation.Results;

namespace PrintHub.WPF.Shared.ViewModels;

public abstract class ValidationViewModel<TV>(IValidator<TV> validator) : ViewModelBase, INotifyDataErrorInfo
{
    protected abstract TV ViewModel { get; }

    protected override bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        bool isSet = base.Set(ref field, value, propertyName);

        if (propertyName is not null)
            ValidateProperty(propertyName);

        return isSet;
    }

    protected void Validate()
    {
        ClearErrors();

        ValidationResult result = validator.Validate(ViewModel);

        IEnumerable<ValidationFailure> errors = result.Errors.DistinctBy(failure => failure.PropertyName);

        foreach (ValidationFailure error in errors)
            AddError(error.PropertyName, error.ErrorMessage);
    }

    protected void ValidateProperty(string propertyName)
    {
        ClearErrors(propertyName);

        ValidationResult result = validator.Validate(ViewModel, strategy => strategy.IncludeProperties(propertyName));

        if (result.Errors.Count == 0)
            return;

        string firstErrorMessage = result.Errors.First().ErrorMessage;

        AddError(propertyName, firstErrorMessage);
    }

    #region INotifyDataErrorInfo

    private readonly Dictionary<string, List<string>> _propertyErrors = new();

    public bool HasErrors => _propertyErrors.Count != 0;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName) => _propertyErrors!.GetValueOrDefault(propertyName, []);

    private void AddError(string propertyName, string errorMessage)
    {
        if (_propertyErrors.ContainsKey(propertyName) == false)
            _propertyErrors.Add(propertyName, []);

        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(propertyName);
    }

    private void ClearErrors()
    {
        _propertyErrors.Clear();
        OnErrorsChanged();
    }

    private void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }

    private void OnErrorsChanged(string? propertyName = null)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    #endregion
}