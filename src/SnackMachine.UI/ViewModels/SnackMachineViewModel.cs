using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackMachine.Domain;
using SnackMachine_ = SnackMachine.Domain.SnackMachine;

namespace SnackMachine.UI.ViewModels
{
    public class SnackMachineViewModel
    {
        private readonly SnackMachine_ _snackMachine;

        public string MoneyInTransaction => _snackMachine.MoneyInTransaction.ToString();
        public Money MoneyInside => _snackMachine.MoneyInside + _snackMachine.MoneyInTransaction;

        public SnackMachineViewModel(SnackMachine_ snackMachine)
        {
            _snackMachine = snackMachine;
        }
    }
}
