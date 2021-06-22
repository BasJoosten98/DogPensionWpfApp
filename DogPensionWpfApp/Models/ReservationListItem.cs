using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DogPensionWpfApp.Models
{
    public class ReservationListItem
    {
        public string DogName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public ICommand ButtonDetailsCommand { get; set; }
    }
}
