using SnackMachine.Domain;

namespace SnackMachine.UI.ViewModels
{
    public class SnackPileViewModel
    {
        private readonly SnackPile _snackPile;

        public string Price => _snackPile.Price.ToString("C2");
        public int Amount => _snackPile.Quantity;
        public string Name => _snackPile.Snack.Name;

        public SnackPileViewModel(SnackPile snackPile)
        {
            _snackPile = snackPile;
        }
    }
}