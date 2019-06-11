using System.Collections.Generic;
using System.Linq;
using SnackMachine.Domain;
using SnackMachine_ = SnackMachine.Domain.SnackMachine;

namespace SnackMachine.UI.ViewModels
{
    public class SnackMachineViewModel
    {
        private readonly SnackMachine_ _snackMachine;

        public string MoneyInTransaction => _snackMachine.MoneyInTransaction.ToString();
        public Money MoneyInside => _snackMachine.MoneyInside;
        public string ErrorMessage { get; set; }

        public IReadOnlyList<SnackPileViewModel> Piles
        {
            get
            {
                return _snackMachine.GetAllSnackPiles().Select(x => new SnackPileViewModel(x)).ToList();
            }
        }

        public SnackMachineViewModel(SnackMachine_ snackMachine)
        {
            _snackMachine = snackMachine;
        }
    }
}