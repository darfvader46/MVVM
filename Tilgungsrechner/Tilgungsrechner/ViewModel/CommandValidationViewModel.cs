using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilgungsrechner.ViewModel
{
    public class CommandValidationViewModel : NotifyDataErrorInfoBase
    {
        // Eingaben
        private decimal _darlehen;
        private decimal _tilgungsrate;
        private decimal _zinsfuss;

        public decimal Darlehen
        {
            get { return _darlehen; }
            set
            {
                SetProperty(ref _darlehen, value);
                ClearErrors(nameof(Darlehen));
                OnInitialisierenCanExecute();
                OnBerechnenCanExecute();
            }
        }

        public decimal Tilgungsrate
        {
            get { return _tilgungsrate; }
            set
            {
                SetProperty(ref _tilgungsrate, value);
                ClearErrors(nameof(Tilgungsrate));
                if(_tilgungsrate < 1 || _tilgungsrate > 100)
                {
                    AddError(nameof(Tilgungsrate), "Die Tilgungsrate muss zwischen 1 und 100 liegen");
                }
                OnInitialisierenCanExecute();
                OnBerechnenCanExecute();
            }
        }

        public decimal Zinsfuss
        {
            get { return _zinsfuss; }
            set
            {
                SetProperty(ref _zinsfuss, value);
                ClearErrors(nameof(Zinsfuss));
                if(_zinsfuss < 0 || _zinsfuss > 25)
                {
                    AddError(nameof(Zinsfuss), "Der Zinsfuß muss zischen 0 und 25 liegen");
                }
                OnInitialisierenCanExecute();
                OnBerechnenCanExecute();
            }
        }

        // Ausgaben
        private decimal _annuitaet;
        public decimal Annuitaet
        {
            set
            {
                SetProperty(ref _annuitaet, value);
                OnBerechnenCanExecute();
                OnInitialisierenCanExecute();
            }
            get
            {
                return _darlehen * (_zinsfuss + _tilgungsrate);
            }
        }
        public ObservableCollection<Jahresplan> Tilgungsplan { get; }

        // Aktionen
        public DelegateCommand InitialisierenCommand { get; }
        public DelegateCommand BerechnenCommand { get; }

        public CommandValidationViewModel()
        {
            Tilgungsplan = new ObservableCollection<Jahresplan>();
            InitialisierenCommand = new DelegateCommand(OnInitialiserenExcecute, OnInitialisierenCanExecute);
            BerechnenCommand = new DelegateCommand(OnBerechnenExecute, OnBerechnenCanExecute);
        }

        private bool OnBerechnenCanExecute()
        {
            return !HasErrors;
        }

        private void OnBerechnenExecute()
        {
            decimal rest = _darlehen;
            int jahr = 1;
            decimal betragZinsen = _darlehen * (_zinsfuss / 100);
            decimal betragTilgung = _darlehen * (_tilgungsrate / 100);
            while(rest > 0)
            {
                Jahresplan jp = new Jahresplan(jahr, rest, betragZinsen, betragTilgung, betragZinsen + betragTilgung);
                Tilgungsplan.Add(jp);
                rest -= betragTilgung;
                betragZinsen = rest * (_zinsfuss / 100);
                betragTilgung = betragTilgung + (jp.Zinsen - betragZinsen);
                jahr++;
            }


        }

        private void OnInitialiserenExcecute()
        {
            
        }

        private bool OnInitialisierenCanExecute()
        {
            return true;   
        }
    }
}
