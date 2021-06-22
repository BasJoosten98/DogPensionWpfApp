using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Models.Wrappers;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogPensionWpfApp.ViewModels
{
    public class ReservationDetailsViewModel : ViewModelBase
    {
        private readonly OwnerService ownerService;
        private readonly DogService dogService;
        private readonly ReservationService reservationService;
        private readonly Navigator navigator;
        private readonly Authenticator authenticator;

        private bool reloadDogsOnNewSelectedOwner = false;

        #region Properties
        private ReservationWrapper reservationWrapper;

        public ReservationWrapper Reservation
        {
            get { return reservationWrapper; }
            set { reservationWrapper = value; OnPropertyChanged(nameof(Reservation)); }
        }
        private List<Owner> ownersList;
        public List<Owner> OwnersList
        {
            get { return ownersList; }
            set { ownersList = value; OnPropertyChanged(nameof(OwnersList)); }
        }
        private Dog selectedDog;
        public Dog SelectedDog
        {
            get => selectedDog;
            set
            {
                Reservation.DogId = value != null ? value.Id : 0;
                selectedDog = value;
                OnPropertyChanged(nameof(SelectedDog));
            }
        }
        private List<Dog> dogsList;
        public List<Dog> DogsList
        {
            get { return dogsList; }
            set { dogsList = value; OnPropertyChanged(nameof(DogsList)); }
        }
        private Owner selectedOwner;
        public Owner SelectedOwner
        {
            get { return selectedOwner; }
            set { selectedOwner = value; 
                OnPropertyChanged(nameof(SelectedOwner));
                if (reloadDogsOnNewSelectedOwner) { reloadDogsList(value.Id); }
            }
        }
        private string errors;
        public string Errors
        {
            get { return errors; }
            set { errors = value; OnPropertyChanged(nameof(Errors)); OnPropertyChanged(nameof(HasErrors)); }
        }
        public bool HasErrors => !string.IsNullOrEmpty(Errors);
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        #endregion

        public ReservationDetailsViewModel(OwnerService ownerService, DogService dogService, ReservationService reservationService, Navigator navigator, Authenticator authenticator)
        {
            this.ownerService = ownerService;
            this.dogService = dogService;
            this.reservationService = reservationService;
            this.navigator = navigator;
            this.authenticator = authenticator;

            SaveCommand = new SaveCommand<Reservation>(reservationService, () => Reservation, () => navigator.SetViewModelOnMain(RegisteredViewModel.ReservationList), displayErrors);
            DeleteCommand = new DeleteCommand<Reservation>(reservationService, () => Reservation, () => navigator.SetViewModelOnMain(null), displayErrors);
        }

        public override async void Load(object arguments)
        {
            if(arguments is int)//reservation id
            {
                await loadReservationById((int)arguments);
            }
            else if (arguments is Reservation)
            {
                await loadReservation((Reservation)arguments);
            }
            else
            {
                await loadReservation();
            }
        }
        private void displayErrors(List<ValidationResult> results)
        {
            string errors = "";
            foreach (ValidationResult result in results)
            {
                if (string.IsNullOrEmpty(errors)) { errors = result.ErrorMessage; }
                else { errors += "\n" + result.ErrorMessage; }
            }
            Errors = errors;
        }

        private async void reloadDogsList(int ownerId)
        {
            var dogsListResult = await dogService.GetAllDogsWithOwner(ownerId);
            DogsList = dogsListResult.ToList();
        }

        private async Task loadReservationById(int id)
        {
            Reservation reservation = await reservationService.GetReservationById(id);
            await loadReservation(reservation);
        }
        private async Task loadReservation(Reservation reservation = null)
        {
            var ownersResult = await ownerService.GetAllOwners();
            await Task.Delay(250);

            if (reservation != null)
            {
                Reservation = new ReservationWrapper(reservation);
                Dog dog = await dogService.GetDogById(reservation.DogId);
                if(dog != null)
                {
                    Owner owner = await ownerService.GetOwnerById(dog.OwnerId);
                    var dogsListResult = await dogService.GetAllDogsWithOwner(owner.Id);

                    DogsList = dogsListResult.ToList();
                    SelectedDog = dog;
                    SelectedOwner = owner;
                }
            }
            else
            {
                Reservation = new ReservationWrapper(new Reservation());
                Reservation.EnterDate = DateTime.Today;
                Reservation.LeaveDate = DateTime.Today;
            }

            OwnersList = ownersResult.ToList();
            reloadDogsOnNewSelectedOwner = true;
        }
    }
}
