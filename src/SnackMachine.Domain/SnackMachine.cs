using System;
using System.Collections.Generic;
using System.Linq;

namespace SnackMachine.Domain
{
    public class SnackMachine : Entity
    {
        public Money MoneyInside { get; private set; } = Money.None;
        public Money MoneyInTransaction { get; private set; } = Money.None;
        public List<Slot> Slots { get; private set; }

        public SnackMachine()
        {
            Slots = new List<Slot>
            {
                new Slot(this, null, 0, 0, 1),
                new Slot(this, null, 0, 0, 2),
                new Slot(this, null, 0, 0, 3),
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
            slot.Quantity--;

            MoneyInside += MoneyInTransaction;

            MoneyInTransaction = Money.None;
        }

        public void LoadSnacks(int position, Snack snack, int quantity, decimal price)
        {
            var slot = Slots.Single(x => x.Position == position);
            slot.Snack = snack;
            slot.Quantity = quantity;
            slot.Price = price;
        }
    }
}