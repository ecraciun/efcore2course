using System;
using System.Collections.Generic;
using System.Text;

namespace SnackMachine.Domain
{
    public class HeadOffice : AggregateRoot
    {
        public decimal SalesAmount { get; private set; }

        public void ChangeSalesAmount(decimal delta)
        {
            SalesAmount += delta;
        }
    }
}
