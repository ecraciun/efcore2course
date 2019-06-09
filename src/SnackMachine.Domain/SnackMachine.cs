using System;

namespace SnackMachine.Domain
{
    public class SnackMachine : Entity
    {
        public Money MoneyInside { get; private set; }
        public Money MoneyInTransaction { get; private set; }


        public SnackMachine()
        {
            MoneyInside = new Money(0, 0, 0, 0, 0, 0);
            MoneyInTransaction = new Money(0, 0, 0, 0, 0, 0);
        }

        public void InsertMoney(Money money)
        {
            MoneyInTransaction += money;
        }

        public void ReturnMoney()
        {
            MoneyInTransaction = new Money(0, 0, 0, 0, 0, 0);
        }

        public void BuySnack()
        {
            MoneyInside += MoneyInTransaction;

            MoneyInTransaction = new Money(0, 0, 0, 0, 0, 0);
        }
    }
}