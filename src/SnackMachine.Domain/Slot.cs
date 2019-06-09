using System;
using System.Collections.Generic;
using System.Text;

namespace SnackMachine.Domain
{
    public class Slot
    {
        public Snack Snack { get;  set; }
        public int Quantity { get;  set; }
        public decimal Price { get;  set; }
        public SnackMachine SnackMachine { get; private set; }
        public int Position { get; private set; }

        private Slot() { }

        public Slot(
            SnackMachine snackMachine,
            Snack snack,
            int quantity,
            decimal price,
            int position)
        {
            Snack = snack;
            SnackMachine = snackMachine;
            Position = position;
            Price = price;
            Quantity = quantity;
        }
    }
}