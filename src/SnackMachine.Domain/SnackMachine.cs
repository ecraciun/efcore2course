using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackMachine.Domain
{
    public class SnackMachine : AggregateRoot
    {
        public Money MoneyInside { get; private set; } = Money.None;
        public decimal MoneyInTransaction { get; private set; } = 0m;
        protected List<Slot> Slots { get; private set; } = new List<Slot>();

        public SnackMachine()
        {
        }


        public void InsertMoney(Money money)
        {
            Money[] coinsAndNotes =
            {
                Money.Cent, Money.TenCent, Money.Quarter, Money.Dollar, Money.FiveDollar, Money.TwentyDollar
            };
            if (!coinsAndNotes.Contains(money))
                throw new InvalidOperationException();

            MoneyInTransaction += money.Amount;
            MoneyInside += money;
        }

        public void ReturnMoney()
        {
            var moneyToReturn = MoneyInside.Allocate(MoneyInTransaction);
            MoneyInside -= moneyToReturn;
            MoneyInTransaction = 0m;
        }

        public void BuySnack(int position)
        {
            if (CanBuySnack(position) != string.Empty)
                throw new InvalidOperationException();

            var slot = GetSlot(position);
            slot.SnackPile = slot.SnackPile.SubtractOne();

            var change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
            MoneyInside -= change;
            MoneyInTransaction = 0m;
        }

        public void LoadSnacks(int position, SnackPile snackPile)
        {
            var slot = GetSlot(position);
            slot.SnackPile = snackPile;
        }

        public SnackPile GetSnackPile(int position)
        {
            return GetSlot(position).SnackPile;
        }

        private Slot GetSlot(int position)
        {
            return Slots.Single(x => x.Position == position);
        }

        public void LoadMoney(Money money)
        {
            MoneyInside += money;
        }

        public IReadOnlyList<SnackPile> GetAllSnackPiles()
        {
            return Slots.OrderBy(x => x.Position).Select(x => x.SnackPile).ToList();
        }

        public string CanBuySnack(int position)
        {
            var snackPile = GetSnackPile(position);

            if (snackPile.Quantity == 0)
                return "The snack pile is empty";

            if (MoneyInTransaction < snackPile.Price)
                return "Not enough money";

            if (!MoneyInside.CanAllocate(MoneyInTransaction - snackPile.Price))
                return "Not enough change";

            return string.Empty;
        }
    }
}