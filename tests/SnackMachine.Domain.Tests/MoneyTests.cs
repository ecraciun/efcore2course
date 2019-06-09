using FluentAssertions;
using System;
using Xunit;

namespace SnackMachine.Domain.Tests
{
    public class MoneyTests
    {
        [Fact]
        public void SumOfTheMoneysProducesCorrectResult()
        {
            Money m1 = new Money(1, 2, 3, 4, 5, 6);
            Money m2 = new Money(1, 2, 3, 4, 5, 6);

            Money sum = m1 + m2;

            sum.OneCentCount.Should().Be(2);
            sum.TenCentCount.Should().Be(4);
            sum.QuarterCount.Should().Be(6);
            sum.OneDollarCount.Should().Be(8);
            sum.FiveDollarCount.Should().Be(10);
            sum.TwentyDollarCount.Should().Be(12);
        }

        [Fact]
        public void TwoMoneyInstancesEqualIfTheyContainTheSameAmount()
        {
            Money m1 = new Money(1, 2, 3, 4, 5, 6);
            Money m2 = new Money(1, 2, 3, 4, 5, 6);

            m1.Should().Be(m2);
            m1.GetHashCode().Should().Be(m2.GetHashCode());
        }

        [Fact]
        public void TwoMoneyInstancesDoNotEqualIfTheyContainDifferentAmounts()
        {
            Money dollar = new Money(0, 0, 0, 1, 0, 0);
            Money hundredCents = new Money(100, 0, 0, 0, 0, 0);

            dollar.Should().NotBe(hundredCents);
            dollar.GetHashCode().Should().NotBe(hundredCents.GetHashCode());
        }

        [Theory]
        [InlineData(-1, 0, 0, 0, 0, 0)]
        [InlineData(0, -2, 0, 0, 0, 0)]
        [InlineData(0, 0, -3, 0, 0, 0)]
        [InlineData(0, 0, 0, -4, 0, 0)]
        [InlineData(0, 0, 0, 0, -5, 0)]
        [InlineData(0, 0, 0, 0, 0, -6)]
        public void CannotCreateMoneyWithNeagativValue(
            int oneCentCount,
            int tenCentCount,
            int quarterCount,
            int oneDollarCount,
            int fiveDollarCount,
            int twentyDollarCount)
        {
            Action action = () => new Money(
                oneCentCount,
                tenCentCount,
                quarterCount,
                oneDollarCount,
                fiveDollarCount,
                twentyDollarCount);

            Assert.Throws<InvalidOperationException>(action);
        }
    }
}