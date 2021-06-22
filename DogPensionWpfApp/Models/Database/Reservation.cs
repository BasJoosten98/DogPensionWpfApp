using DogPensionWpfApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DogPensionWpfApp.Models.Database
{
    public class Reservation : WrapperModelBase
    {
        public int Id { get; set; }

        [Required]
        public DateTime EnterDate { get; set; }

        [Required]
        public DateTime LeaveDate { get; set; }

        [Required]
        public int DogId { get; set; }


        public virtual Dog Dog { get; set; }

    }
}
