using System;
using System.Collections.Generic;
using System.Text;

namespace SnackMachine.Domain
{
    public class HeadOfficeRepository : Repository<HeadOffice>
    {
        public HeadOfficeRepository(SnackMachineContext context) : base(context)
        {
        }
    }
}
