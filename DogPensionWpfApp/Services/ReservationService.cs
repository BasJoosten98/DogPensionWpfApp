using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.Services
{
    public class ReservationService : ICRUD<Reservation>
    {
        private readonly DogPensionDbContextFactory factory;
        private readonly Authenticator authenticator;

        public ReservationService(DogPensionDbContextFactory factory, Authenticator authenticator)
        {
            this.factory = factory;
            this.authenticator = authenticator;
        }

        public async Task<Reservation> GetReservationById(int id)
        {
            using (ReservationRepository repo = new ReservationRepository(factory.CreateDbContext()))
            {
                Reservation model = await repo.GetByIdAsync(id);
                return model;
            }
        }
        public async Task<IEnumerable<Reservation>> GetReservationBetweenDates(DateTime start, DateTime end)
        {
            using (ReservationRepository repo = new ReservationRepository(factory.CreateDbContext()))
            {
                IEnumerable<Reservation> model = await repo.GetReservationsBetweenDate(start, end, authenticator);
                return model;
            }
        }
        public async Task<Reservation> Create(Reservation model)
        {
            using (ReservationRepository repo = new ReservationRepository(factory.CreateDbContext()))
            {
                IEnumerable<Reservation> closeReservations = await repo.GetReservationsBetweenDate(model.EnterDate, model.LeaveDate, authenticator);
                closeReservations = closeReservations.Where(r => r.DogId == model.DogId);
                bool inConflict = closeReservations.Any(r => (r.EnterDate.Date >= model.EnterDate && r.EnterDate.Date <= model.LeaveDate) || (r.LeaveDate.Date >= model.EnterDate && r.LeaveDate.Date <= model.LeaveDate));
                if (inConflict) { throw new Exception("Reservation date is in conflict with other reservations"); }

                repo.Add(model);
                await repo.SaveAsync();
            }
            return model;
        }

        public async Task<Reservation> CreateOrUpdate(Reservation model)
        {
            if (model.Id == 0) { return await Create(model); }
            else { return await Update(model); }
        }

        public async Task Remove(Reservation model)
        {
            using (ReservationRepository repo = new ReservationRepository(factory.CreateDbContext()))
            {
                repo.Remove(model);
                await repo.SaveAsync();
            }
        }

        public async Task<Reservation> Update(Reservation model)
        {
            using (ReservationRepository repo = new ReservationRepository(factory.CreateDbContext()))
            {
                IEnumerable<Reservation> closeReservations = await repo.GetReservationsBetweenDate(model.EnterDate, model.LeaveDate, authenticator);
                closeReservations = closeReservations.Where(r => r.DogId == model.DogId && r.Id != model.Id);
                bool inConflict = closeReservations.Any(r => (r.EnterDate.Date >= model.EnterDate && r.EnterDate.Date <= model.LeaveDate) || (r.LeaveDate.Date >= model.EnterDate && r.LeaveDate.Date <= model.LeaveDate));
                if (inConflict) { throw new Exception("Reservation date is in conflict with other reservations"); }

                repo.Update(model);
                await repo.SaveAsync();
            }
            return model;
        }
    }
}
