using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static SnackMachine.Domain.Money;

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

            sm.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void InsertedMoneyGoesToMoneyInTransaction()
        {
            var snackMachine = new SnackMachine();

            snackMachine.InsertMoney(Money.Cent);
            snackMachine.InsertMoney(Money.Dollar);

            snackMachine.MoneyInTransaction.Should().Be(1.01m);
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
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Chocolate"), 10, 1m));
            snackMachine.InsertMoney(Dollar);
            snackMachine.InsertMoney(Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(0m);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
        }

        [Fact]
        public void BuySnackTradesInsertedMoneyForASnack()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Chocolate"), 10, 1m));
            snackMachine.InsertMoney(Dollar);

            snackMachine.BuySnack(1);

            snackMachine.MoneyInTransaction.Should().Be(0m);
            snackMachine.MoneyInside.Amount.Should().Be(1m);
            //snackMachine.GetQuantityOfSnacksInSlot(1).Should().Be(9);
            //snackMachine.GetSnackInSlot(1).Should().Be(new Snack("Chocolate"));
            //snackMachine.GetPriceInSlot(1).Should().Be(1m);
            snackMachine.GetSnackPile(1).Quantity.Should().Be(9);
        }

        [Fact]
        public void CannotMakePurchaseWhenThereIsNoSnacks()
        {
            var snackMachine = new SnackMachine();

            Action action = () => snackMachine.BuySnack(1);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void CannotMakePurchaseIfNotEnoughMoneyInserted()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Chocolate"), 1, 2m));
            snackMachine.InsertMoney(Dollar);

            Action action = () => snackMachine.BuySnack(1);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void SnackMachineReturnsMoneyWithHighestDenominationFirst()
        {
            SnackMachine snackMachine = new SnackMachine();
            snackMachine.LoadMoney(Dollar);

            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.InsertMoney(Quarter);
            snackMachine.ReturnMoney();

            snackMachine.MoneyInside.QuarterCount.Should().Be(4);
            snackMachine.MoneyInside.OneDollarCount.Should().Be(0);
        }

        [Fact]
        public void AfterPurchaseChangeIsReturned()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Chocolate"), 1, 0.5m));
            snackMachine.LoadMoney(TenCent * 10);

            snackMachine.InsertMoney(Dollar);
            snackMachine.BuySnack(1);

            snackMachine.MoneyInside.Amount.Should().Be(1.5m);
            snackMachine.MoneyInTransaction.Should().Be(0m);
        }

        [Fact]
        public void CannotBuySnackIfNotEnoughChange()
        {
            var snackMachine = new SnackMachine();
            snackMachine.LoadSnacks(1, new SnackPile(new Snack("Chocolate"), 1, 0.5m));
            snackMachine.InsertMoney(Dollar);

            Action action = () => snackMachine.BuySnack(1);

            Assert.Throws<InvalidOperationException>(action);
        }
    }
}
