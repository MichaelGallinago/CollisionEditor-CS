using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using System;

namespace CollisionEditor.View
{
    internal class TextboxValidator : INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public bool HasErrors => propertyErrors.Any();

        private readonly Dictionary<string, List<string>> propertyErrors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string? propertyName)
        {
            return propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!propertyErrors.ContainsKey(propertyName))
                propertyErrors.Add(propertyName, new List<string>());

            propertyErrors[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void ClearErrors(string propertyName)
        {
            if (propertyErrors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }
    }
}
