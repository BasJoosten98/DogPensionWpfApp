using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models;
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
using System.Windows.Media.Imaging;

namespace DogPensionWpfApp.ViewModels
{
    public class OwnerDetailsViewModel : ViewModelBase
    {
        private readonly OwnerService ownerService;
        private readonly DogService dogService;
        private readonly Navigator navigator;
        private OwnerWrapper owner;
        public OwnerWrapper Owner
        {
            get { return owner; }
            set { owner = value; OnPropertyChanged(nameof(Owner)); }
        }

        private List<OwnerDogListItem> _itemList;
        public List<OwnerDogListItem> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; OnPropertyChanged(nameof(ItemList)); }
        }
        private BitmapImage profilePictureSource;

        public BitmapImage ProfilePictureSource
        {
            get { return profilePictureSource; }
            set { profilePictureSource = value; OnPropertyChanged(nameof(ProfilePictureSource)); }
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
        public ICommand CreateNewDogCommand { get; set; }
        public ICommand ChangePictureCommand { get; set; }

        public OwnerDetailsViewModel(OwnerService ownerService, DogService dogService, Navigator navigator)
        {
            this.ownerService = ownerService;
            this.dogService = dogService;
            this.navigator = navigator;

            ProfilePictureSource = new BitmapImage(new Uri("pack://application:,,,/Resources/no_image.jpg"));

            SaveCommand = new SaveCommand<Owner>(ownerService, () => Owner, () => navigator.SetViewModelOnMain(RegisteredViewModel.OwnerList), (errors) => displayErrors(errors));
            DeleteCommand = new DeleteCommand<Owner>(ownerService, () => Owner, () => navigator.SetViewModelOnMain(null), (errors) => displayErrors(errors));
            CreateNewDogCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.DogDetails, new Dog { OwnerId = Owner.Owner.Id}));
            ChangePictureCommand = new GetPictureCommand(gettingImageFromUser);
        }

        private void gettingImageFromUser(ProfilePicture profilePicture, BitmapImage image)
        {
            if (Owner == null) { Errors = "No model has been loaded yet. Picture can not be set."; return; }
            Owner.Model.ProfilePicture = profilePicture;
            Owner.HasChanges = true;
            ProfilePictureSource = image;
        }

        private void displayErrors(List<ValidationResult> results)
        {
            string errors = "";
            foreach(ValidationResult result in results)
            {
                if (string.IsNullOrEmpty(errors)) { errors = result.ErrorMessage; }
                else { errors += "\n" + result.ErrorMessage; }
            }
            Errors = errors;
        }

        public override async void Load(object arguments)
        {
            if(arguments is int)
            {
                await loadOwnerById((int)arguments);
            }
            else
            {
                Owner = new OwnerWrapper(new Owner());
            }
        }

        private async Task loadOwnerById(int id)
        {
            Owner owner = await ownerService.GetOwnerByIdIncludingProfilePicture(id);
            await Task.Delay(250);
            if (owner != null)
            {
                Owner = new OwnerWrapper(owner);
                if(owner.ProfilePicture != null) { ProfilePictureSource = ImageBytes.ConvertBytesToBitmapImage(owner.ProfilePicture.Picture); }

                //Load dogs
                IEnumerable<Dog> dogsOfOwner = await dogService.GetAllDogsWithOwnerIncludingProfilePicture(id);
                await Task.Delay(250);
                List<OwnerDogListItem> items = new List<OwnerDogListItem>();
                foreach (Dog dog in dogsOfOwner)
                {
                    OwnerDogListItem item = new OwnerDogListItem();
                    item.Name = dog.Name;
                    item.ButtonClickCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.DogDetails, dog.Id));
                    if (dog.ProfilePicture != null) { item.PictureSource = ImageBytes.ConvertBytesToBitmapImage(dog.ProfilePicture.Picture); }
                    items.Add(item);
                }
                ItemList = items;
            }
        }
    }
}
