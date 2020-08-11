using CommandBeispiel.ViewModel;

namespace Tilgungsrechner.ViewModel
{
    // Stellt eine Jahresplan eines Tilgungsplan dar
    public class Jahresplan : NotifyDataErrorInfoBase
    {
        #region Attribute
        private int _jahr;
        private decimal? _restschuldAmAnfang;
        private decimal? _zinsen;
        private decimal? _tilgung;
        private decimal? _annuitaet;
        public int Jahr
        {
            get { return _jahr; }
            set { _jahr = value; }
        }

        public decimal? RestSchuldAmAnfang
        {
            get { return _restschuldAmAnfang; }
            set { _restschuldAmAnfang = value; }
        }

        public decimal? Zinsen
        {
            get { return _zinsen; }
            set { _zinsen = value; }
        }

        public decimal? Tilgung
        {
            get { return _tilgung; }
            set { _tilgung = value; }
        }
        
        public decimal? Annuitaet
        {
            get { return _annuitaet; }
            set { _annuitaet = value; }
        }
        #endregion

        #region Konstruktoren
        public Jahresplan(int jahr, decimal? restschuldAmAnfang, decimal? zinsen, decimal? tilgung, decimal? annuitaet)
        {
            _jahr = jahr;
            _restschuldAmAnfang = restschuldAmAnfang;
            _zinsen = zinsen;
            _tilgung = tilgung;
            _annuitaet = annuitaet;
        }
        #endregion
    }
}
