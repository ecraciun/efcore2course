using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain;
using SnackMachine.UI.ViewModels;
using SnackMachine_ = SnackMachine.Domain.SnackMachine;

namespace SnackMachine.UI.Controllers
{
    public class HomeController : Controller
    {
        private const string MainViewName = nameof(Index);
        private static SnackMachine_ SnackMachine = new SnackMachine_();

        public IActionResult Index()
        {
            return View(new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertCent()
        {
            SnackMachine.InsertMoney(Money.Cent);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertTenCent()
        {
            SnackMachine.InsertMoney(Money.TenCent);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertQuarter()
        {
            SnackMachine.InsertMoney(Money.Quarter);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertDollar()
        {
            SnackMachine.InsertMoney(Money.Dollar);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertFiveDollar()
        {
            SnackMachine.InsertMoney(Money.FiveDollar);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult InsertTwentyDollar()
        {
            SnackMachine.InsertMoney(Money.TwentyDollar);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult ReturnMoeny()
        {
            SnackMachine.ReturnMoney();
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult BuySnack()
        {
            SnackMachine.BuySnack();
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }
    }
}