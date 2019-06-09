using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackMachine.Domain
{
    public class SnackMachine : AggregateRoot
    {
        public Money MoneyInside { get; private set; } = Money.None;
        public Money MoneyInTransaction { get; private set; } = Money.None;
        protected List<Slot> Slots { get; private set; }

        public SnackMachine()
        {
            Slots = new List<Slot>
            {
                new Slot(this, 1),
                new Slot(this, 2),
                new Slot(this, 3),
            };
        }


        public void InsertMoney(Money money)
        {
            Money[] coinsAndNotes =
            {
                Money.Cent, Money.TenCent, Money.Quarter, Money.Dollar, Money.FiveDollar, Money.TwentyDollar
            };
            if (!coinsAndNotes.Contains(money))
                throw new InvalidOperationException();

            MoneyInTransaction += money;
        }

        public void ReturnMoney()
        {
            MoneyInTransaction = Money.None;
        }

        public void BuySnack(int position)
        {
            var slot = Slots.Single(x => x.Position == position);
            slot.SnackPile = slot.SnackPile.SubtractOne();

            MoneyInside += MoneyInTransaction;

            MoneyInTransaction = Money.None;
        }

        public void LoadSnacks(int position, SnackPile snackPile)
        {
            var slot = Slots.Single(x => x.Position == position);
            slot.SnackPile = snackPile;
        }

        public SnackPile GetSnackPile(int position)
        {
            return Slots.Single(x => x.Position == position).SnackPile;
        }
    }
}