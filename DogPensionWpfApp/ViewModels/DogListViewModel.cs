using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogPensionWpfApp.ViewModels
{
    public class DogListViewModel : ViewModelBase
    {
        private readonly DogService dogService;
        private readonly Navigator navigator;
        private List<OwnerDogListItem> _itemList;
        public List<OwnerDogListItem> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; OnPropertyChanged(nameof(ItemList)); }
        }

        public DogListViewModel(DogService dogService, Navigator navigator)
        {
            this.dogService = dogService;
            this.navigator = navigator;
        }

        public override async void Load(object arguments)
        {
            await loadAllDogs();
        }

        private async Task loadAllDogs()
        {
            IEnumerable<Dog> dogs = await dogService.GetAllDogsIncludingProfilePicture();
            await Task.Delay(250);
            List<OwnerDogListItem> items = new List<OwnerDogListItem>();
            foreach(Dog dog in dogs)
            {
                OwnerDogListItem item = new OwnerDogListItem();
                item.Name = dog.Name;
                item.ButtonClickCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.DogDetails, dog.Id));
                if(dog.ProfilePicture != null) { item.PictureSource = ImageBytes.ConvertBytesToBitmapImage(dog.ProfilePicture.Picture); }
                items.Add(item);
            }
            ItemList = items;
        }
    }
}
