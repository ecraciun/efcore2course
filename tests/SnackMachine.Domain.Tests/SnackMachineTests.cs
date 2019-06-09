using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SnackMachine.Domain.Tests
{
    public class SnackMachineTests
    {
        [Fact]
        public void ReturnMoneyEmptiesMoneyInTransaction()
        {
            var sm = new SnackMachine();
            sm.InsertMoney(Money.Dollar);

            sm.ReturnMoney();

            sm.MoneyInTransaction.Amount.Should().Be(0m);
        }

        [Fact]
        public void InsertedMoneyGoesToMoneyInTransaction()
        {
            var snackMachine = new SnackMachine();

            snackMachine.InsertMoney(Money.Cent);
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.MoneyInTransaction.Amount.Should().Be(1.01m);
        }

        [Fact]
        public void CannotInsertMoreThanOneCoinOrNoteAtATime()
        {
            var snackMachine = new SnackMachine();
            Money twoCent = Money.Cent + Money.Cent;

            Action action = () => snackMachine.InsertMoney(twoCent);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void MoneyInTransactionGoesToMoneyInsideAfterPurchase()
        {
            var snackMachine = new SnackMachine();
            snackMachine.InsertMoney(Money.Dollar);
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(Money.None);
            snackMachine.MoneyInside.Amount.Should().Be(2m);
        }

        [Fact]
        public void BuySnackTradesInsertedMoneyForASnack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new Snack("Chocolate"), 10, 1m);
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Amount.Should().Be(0m);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
        }
    }
}
