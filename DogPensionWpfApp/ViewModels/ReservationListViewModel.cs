using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogPensionWpfApp.ViewModels
{
    public class ReservationListViewModel : ViewModelBase
    {
        private readonly DogService dogService;
        private readonly ReservationService reservationService;
        private readonly Navigator navigator;

        private bool allowResevationLoading = false;

        public string AmountOfDogs => "Amount of dogs: " + plannedDogsList.Count.ToString();
        private List<OwnerDogListItem> plannedDogsList;
        public List<OwnerDogListItem> PlannedDogsList
        {
            get { return plannedDogsList; }
            set { plannedDogsList = value; OnPropertyChanged(nameof(PlannedDogsList)); OnPropertyChanged(nameof(AmountOfDogs)); }
        }
        private List<ReservationListItem> plannedReservationsList;
        public List<ReservationListItem> PlannedReservationsList
        {
            get { return plannedReservationsList; }
            set { plannedReservationsList = value; OnPropertyChanged(nameof(PlannedReservationsList)); }
        }
        
        private DateTime fromDate;

        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; 
                if(untilDate < fromDate)
                {
                    untilDate = fromDate; OnPropertyChanged(nameof(UntilDate));
                }
                OnPropertyChanged(nameof(FromDate));
                getAndLoadReservations();
            }
        }
        private DateTime untilDate;

        public DateTime UntilDate
        {
            get { return untilDate; }
            set { untilDate = value;
                if (untilDate < fromDate)
                {
                    fromDate = untilDate; OnPropertyChanged(nameof(FromDate));
                }
                OnPropertyChanged(nameof(UntilDate));
                getAndLoadReservations();
            }
        }
        public ICommand NewReservationCommand { get; set; }


        public ReservationListViewModel(DogService dogService, ReservationService reservationService, Navigator navigator)
        {
            this.dogService = dogService;
            this.reservationService = reservationService;
            this.navigator = navigator;

            plannedDogsList = new List<OwnerDogListItem>();
            plannedReservationsList = new List<ReservationListItem>();
            FromDate = DateTime.Now.Date;
            UntilDate = DateTime.Now.AddDays(7).Date;


            NewReservationCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.ReservationDetails, new Reservation { Id = 0, EnterDate = FromDate, LeaveDate = UntilDate }));
        }

        public override async void Load(object arguments)
        {
            allowResevationLoading = true;
            await getAndLoadReservations();
        }

        private async Task getAndLoadReservations()
        {
            if (fromDate <= untilDate && allowResevationLoading)
            {
                IEnumerable<Reservation> reservations = await reservationService.GetReservationBetweenDates(fromDate.Date, untilDate.Date);
                List<OwnerDogListItem> dogList = new List<OwnerDogListItem>();
                List<ReservationListItem> reservationList = new List<ReservationListItem>();
                List<int> addedDogsIds = new List<int>();
                foreach (Reservation r in reservations)
                {
                    Dog dog = await dogService.GetDogByIdIncludingProfilePicture(r.DogId);
                    if (!addedDogsIds.Contains(dog.Id))
                    {
                        OwnerDogListItem dogItem = new OwnerDogListItem
                        {
                            Name = dog.Name,
                            ButtonContent = "See details",
                            ButtonClickCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.DogDetails, dog.Id))
                        };
                        if(dog.ProfilePicture != null) { dogItem.PictureSource = ImageBytes.ConvertBytesToBitmapImage(dog.ProfilePicture.Picture); }
                        dogList.Add(dogItem);
                        addedDogsIds.Add(dog.Id);
                    }

                    ReservationListItem reservationItem = new ReservationListItem
                    {
                        DogName = dog.Name,
                        StartDate = r.EnterDate.Date.ToString("D"),
                        EndDate = r.LeaveDate.Date.ToString("D"),
                        ButtonDetailsCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.ReservationDetails, r.Id))
                    };
                    reservationList.Add(reservationItem);
                }
                await Task.Delay(250);

                PlannedDogsList = dogList;
                PlannedReservationsList = reservationList;
            }
        }
    }
}
