using System;
using System.Collections.Generic;

namespace SnackMachine.Domain
{
    public class SnackPile : ValueObject<SnackPile>
    {
        public Snack Snack { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }

        private SnackPile() { }

        public SnackPile(Snack snack, int quantity, decimal price)
        {
            Snack = snack;
            Quantity = quantity;
            Price = price;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Snack;
            yield return Quantity;
            yield return Price;
        }

        public SnackPile SubtractOne()
        {
            return new SnackPile(Snack, Quantity - 1, Price);
        }
    }
}