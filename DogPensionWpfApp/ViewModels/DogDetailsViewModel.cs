using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Models.Wrappers;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DogPensionWpfApp.ViewModels
{
    public class DogDetailsViewModel : ViewModelBase
    {
        private readonly DogService dogService;
        private readonly OwnerService ownerService;
        private readonly Navigator navigator;
        private DogWrapper dog;
        public DogWrapper Dog
        {
            get { return dog; }
            set { dog = value; OnPropertyChanged(nameof(Dog)); }
        }

        private OwnerDogListItem ownerItem;

        public OwnerDogListItem OwnerItem
        {
            get { return ownerItem; }
            set { ownerItem = value; OnPropertyChanged(nameof(OwnerItem)); }
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
        public ICommand ChangePictureCommand { get; set; }

        public DogDetailsViewModel(DogService dogService, OwnerService ownerService, Navigator navigator)
        {
            this.dogService = dogService;
            this.ownerService = ownerService;
            this.navigator = navigator;

            ProfilePictureSource = new BitmapImage(new Uri("pack://application:,,,/Resources/no_image.jpg"));

            SaveCommand = new SaveCommand<Dog>(dogService, () => Dog, () => navigator.SetViewModelOnMain(RegisteredViewModel.DogList), (errors) => displayErrors(errors));
            DeleteCommand = new DeleteCommand<Dog>(dogService, () => Dog, () => navigator.SetViewModelOnMain(null), (errors) => displayErrors(errors));
            ChangePictureCommand = new GetPictureCommand(gettingImageFromUser);
        }

        public override async void Load(object arguments)
        {
            if(arguments is int)
            {
                await loadDogById((int)arguments);
            }
            else if(arguments is Dog)
            {
                Dog = new DogWrapper((Dog)arguments);
                await loadOwner();
            }
        }

        private void gettingImageFromUser(ProfilePicture profilePicture, BitmapImage image)
        {
            if(Dog == null) { Errors = "No model has been loaded yet. Picture can not be set."; return; }
            Dog.Model.ProfilePicture = profilePicture;
            Dog.HasChanges = true;
            ProfilePictureSource = image;
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

        private async Task loadDogById(int id)
        {
            Dog dog = await dogService.GetDogByIdIncludingProfilePicture(id);
            await Task.Delay(250);
            Dog = new DogWrapper(dog);
            if (dog.ProfilePicture != null) { ProfilePictureSource = ImageBytes.ConvertBytesToBitmapImage(dog.ProfilePicture.Picture); }
            await loadOwner();
        }
        private async Task loadOwner()
        {
            Owner owner = await ownerService.GetOwnerByIdIncludingProfilePicture(Dog.Dog.OwnerId);
            if(owner != null)
            {
                OwnerDogListItem item = new OwnerDogListItem { Name = owner.Name, ButtonClickCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.OwnerDetails, owner.Id)) };
                if(owner.ProfilePicture != null) { item.PictureSource = ImageBytes.ConvertBytesToBitmapImage(owner.ProfilePicture.Picture); }
                OwnerItem = item;
            }
        }
    }
}
