using CommandBeispiel.ViewModel;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace Tilgungsrechner.ViewModel
{
    public class TilgungsViewModel : NotifyDataErrorInfoBase
    {
        #region Eingaben
        private decimal? _darlehen;
        private decimal? _tilgungsrate;
        private decimal? _zinsfuss;
        private decimal? _annuitaet;

        public decimal? Annuitaet
        {
            get { return _annuitaet; }
            set
            {
                SetProperty(ref _annuitaet, value);
                ClearErrors(nameof(Annuitaet));
            }
        }
        
        public decimal? Darlehen
        {
            get { return _darlehen; }
            set
            {
                SetProperty(ref _darlehen, value);
                ClearErrors(nameof(Darlehen));
                BerechnenCommand.RaiseCanExecuteChanged();
                InitialisierenCommand.RaiseCanExecuteChanged();
            }
        }
        
        public decimal? Tilgungsrate
        {
            get { return _tilgungsrate; }
            set
            {
                SetProperty(ref _tilgungsrate, value);
                ClearErrors(nameof(Tilgungsrate));
                if (_tilgungsrate < 1 || _tilgungsrate > 100)
                {
                    AddError(nameof(Tilgungsrate), "Die Tilgungsrate muss zwischen 1 und 100 liegen");
                }
                BerechnenCommand.RaiseCanExecuteChanged();
                InitialisierenCommand.RaiseCanExecuteChanged();
            }
        }
        
        public decimal? Zinsfuss
        {
            get { return _zinsfuss; }
            set
            {
                SetProperty(ref _zinsfuss, value);
                ClearErrors(nameof(Zinsfuss));
                if (_zinsfuss < 1 || _zinsfuss > 25)
                {
                    AddError(nameof(Zinsfuss), "Der Zinsfuss muss zwischen 1 und 25 liegen");
                }
                BerechnenCommand.RaiseCanExecuteChanged();
                InitialisierenCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Aktionen
        public DelegateCommand BerechnenCommand { get; }
        public DelegateCommand InitialisierenCommand { get; }

        #endregion

        #region Ausgaben
        // Eine ObservableCollection ist in der Lage Änderung zur Laufzeit wahrzunehmen
        // Und diese an der Oberfläche sofort anzupassen
        private ObservableCollection<Jahresplan> _tilgungsplan;

        public ObservableCollection<Jahresplan> Tilgungsplan
        {
            get { return _tilgungsplan; }
            set
            {
                SetProperty(ref _tilgungsplan, value);
            }
        }

        #endregion

        #region Konstruktoren
        public TilgungsViewModel()
        {
            BerechnenCommand = new DelegateCommand(OnBerechnenExecute, OnBerechnenCanExecute);
            InitialisierenCommand = new DelegateCommand(OnInitialiserenExecute, OnInitialisierenCanExecute);
            Tilgungsplan = new ObservableCollection<Jahresplan>();
        }
        #endregion

        #region DelegateCommandMethoden

        // Der Tilgungsplan kann zu jeder Zeit initialisiert werden
        private bool OnInitialisierenCanExecute()
        {
            return true;
        }

        // Zurücksetzen des Tilgungsplans und aller Eingabefelder
        private void OnInitialiserenExecute()
        {
            Darlehen = null;
            Tilgungsrate = null;
            Zinsfuss = null;
            Annuitaet = null;
            Tilgungsplan.Clear();
        }

        // Berechnet den Tilgungsplan aus den Eingaben
        // Greift dadurch auf die ObservableCollection Tilgungsplan zu und manipuliert diese
        private void OnBerechnenExecute()
        {
            Annuitaet = (Darlehen * (Tilgungsrate + Zinsfuss)) / 100;
            int jahr = 1;
            decimal? restschuld = Darlehen;
            decimal? zinsen = Darlehen * Zinsfuss / 100;
            decimal? tilgung = Darlehen * Tilgungsrate / 100;
            decimal? annuitaet = Annuitaet;
            while(restschuld > 0)
            {
                Tilgungsplan.Add(new Jahresplan(jahr, restschuld, zinsen, tilgung, annuitaet));
                restschuld -= tilgung;
                zinsen = Math.Round(Zinsfuss.Value * restschuld.Value / 100, 2, MidpointRounding.AwayFromZero);
                tilgung = Annuitaet - zinsen;
                if(tilgung > restschuld)
                {
                    tilgung = restschuld;
                    annuitaet = zinsen + tilgung; 
                }
                jahr++;
            }
        }

        private bool OnBerechnenCanExecute()
        {
            return !String.IsNullOrEmpty(Darlehen.ToString()) && !String.IsNullOrEmpty(Tilgungsrate.ToString()) && !String.IsNullOrEmpty(Zinsfuss.ToString()) && !HasErrors;
        }
        #endregion
    }
}
