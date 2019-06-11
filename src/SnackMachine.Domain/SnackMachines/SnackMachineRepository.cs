using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SnackMachine.Domain
{
    public class SnackMachineRepository : Repository<SnackMachine>
    {
        public SnackMachineRepository(SnackMachineContext context) : base(context)
        { }

        public override SnackMachine GetById(long id)
        {
            return Context.SnackMachines.Include("Slots").Include("Slots.SnackPile.Snack").FirstOrDefault(x => x.Id == id);
        }
    }
}