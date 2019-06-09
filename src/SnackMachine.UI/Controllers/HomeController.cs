using Microsoft.AspNetCore.Mvc;
using SnackMachine.Domain;
using SnackMachine.UI.ViewModels;
using SnackMachine_ = SnackMachine.Domain.SnackMachine;

namespace SnackMachine.UI.Controllers
{
    public class HomeController : Controller
    {
        private const string MainViewName = nameof(Index);
        private static SnackMachine_ SnackMachine;
        private readonly SnackMachineContext _context;
        private readonly SnackMachineRepository _snackMachineRepository;

        public HomeController(SnackMachineContext context)
        {
            _context = context;
            _snackMachineRepository = new SnackMachineRepository(context);
        }

        public IActionResult Index()
        {
            SnackMachine = _snackMachineRepository.GetById(1);
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

        public IActionResult BuyChocolate()
        {
            SnackMachine.BuySnack(1);
            _snackMachineRepository.Save(SnackMachine);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult BuyCola()
        {
            SnackMachine.BuySnack(2);
            _snackMachineRepository.Save(SnackMachine);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }

        public IActionResult BuyChips()
        {
            SnackMachine.BuySnack(3);
            _snackMachineRepository.Save(SnackMachine);
            return View(MainViewName, new SnackMachineViewModel(SnackMachine));
        }
    }
}