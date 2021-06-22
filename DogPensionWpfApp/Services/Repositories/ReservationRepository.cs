using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation, DogPensionDbContext>
    {
        public ReservationRepository(DogPensionDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Reservation>> GetReservationsBetweenDate(DateTime start, DateTime end, Authenticator auth)
        {
            return await Context.Reservations.Where(r => r.Dog.Owner.AccountId == auth.Account.Id && (r.EnterDate.Date >= start && r.EnterDate.Date <= end) || (r.LeaveDate.Date >= start && r.LeaveDate.Date <= end)).AsNoTracking().ToListAsync();
        }
    }
}
