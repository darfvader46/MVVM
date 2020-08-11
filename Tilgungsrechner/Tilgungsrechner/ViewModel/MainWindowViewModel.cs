using CommandBeispiel.ViewModel;

namespace Tilgungsrechner.ViewModel
{
    // Kapselt ein TilgungsViewModel
    public class MainWindowViewModel : NotifyDataErrorInfoBase
    {
        public TilgungsViewModel TilgungsViewModel { get; }
        public MainWindowViewModel()
        {
            TilgungsViewModel = new TilgungsViewModel();
        }
    }
}
