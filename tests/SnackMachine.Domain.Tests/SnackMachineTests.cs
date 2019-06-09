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
            sm.InsertMoney(new Money(0, 0, 0, 1, 0, 0));

            sm.ReturnMoney();

            sm.MoneyInTransaction.Amount.Should().Be(0m);
        }
    }
}
