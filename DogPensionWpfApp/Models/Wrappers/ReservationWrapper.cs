using DogPensionWpfApp.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DogPensionWpfApp.Models.Wrappers
{
    public class ReservationWrapper : WrapperBase<Reservation>
    {
        public ReservationWrapper(Reservation reservation)
           : base(reservation)
        {
            reservation.EnterDate = reservation.EnterDate.Date;
            reservation.LeaveDate = reservation.LeaveDate.Date;
        }

        public Reservation Reservation { get { return Model; } }

        public DateTime EnterDate
        {
            get { return Model.EnterDate; }
            set { Model.EnterDate = value.Date; OnPropertyChanged(nameof(Model.EnterDate)); }
        }
        public DateTime LeaveDate
        {
            get { return Model.LeaveDate; }
            set { Model.LeaveDate = value.Date; OnPropertyChanged(nameof(Model.LeaveDate)); }
        }
        public int DogId
        {
            get { return Model.DogId; }
            set { Model.DogId = value; OnPropertyChanged(nameof(Model.DogId)); }
        }

        public override List<ValidationResult> GetValidationResults()
        {
            List<ValidationResult> results = base.GetValidationResults();
            if (Model.DogId <= 0) { results.Add(new ValidationResult("No dog has been selected")); }
            if (Model.EnterDate.Date >= Model.LeaveDate.Date) { results.Add(new ValidationResult("End date has to be 1 or more days later than start date.")); }
            return results;
        }
    }
}
