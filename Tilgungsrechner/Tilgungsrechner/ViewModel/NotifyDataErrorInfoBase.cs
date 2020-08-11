using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CommandBeispiel.ViewModel
{
    // Diese Klasse regelt hauptsächlich die Ausgabe der Fehler an der Oberfläche
    public class NotifyDataErrorInfoBase : BindableBase, INotifyDataErrorInfo
    {

        #region INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        // Datenstruktur in der wir pro Eigenschaft mehrere Fehler speichern können
        private Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ? _errorsByPropertyName[propertyName] : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged(nameof(HasErrors));
        }

        // wenn irgendwas da drinnen ist, also irgendein Fehler -> true
        public bool HasErrors => _errorsByPropertyName.Any();

        // Hinzufügen von Fehlern
        protected void AddError(string propertyName, string error)
        {
            // Gibts für den übergebenen PropertyNamen bereits einen Eintrag in meine Dictonary?
            // Wenn nicht, dann muss ich den erstmal hinzufügen
            if (!_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName[propertyName] = new List<string>();
            }

            // Gibts es für diesen PropertyName bereits diesen Fehler?
            // Wenn nicht, dann hinzufügen
            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        // Löschen aller Fehler für eine Eigenschaft
        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        #endregion
    }
}
