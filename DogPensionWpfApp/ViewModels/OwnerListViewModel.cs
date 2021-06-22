using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Models;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DogPensionWpfApp.ViewModels
{
    public class OwnerListViewModel : ViewModelBase
    {
        private readonly OwnerService ownerService;
        private readonly Navigator navigator;
        private List<OwnerDogListItem> _itemList;

        public List<OwnerDogListItem>  ItemList
        {
            get { return _itemList; }
            set { _itemList = value; OnPropertyChanged(nameof(ItemList)); }
        }

        public ICommand CreateNewOwnerCommand { get; set; }

        public OwnerListViewModel(OwnerService ownerService, Navigator navigator)
        {
            this.ownerService = ownerService;
            this.navigator = navigator;

            CreateNewOwnerCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.OwnerDetails));
        }

        public override async void Load(object arguments)
        {
            await loadAllOwners();
        }

        private async Task loadAllOwners()
        {
            IEnumerable<Owner> owners = await ownerService.GetAllOwnersIncludingProfilePicture();
            await Task.Delay(250);
            List<OwnerDogListItem> items = new List<OwnerDogListItem>();
            foreach (Owner owner in owners)
            {
                OwnerDogListItem item = new OwnerDogListItem();
                item.Name = owner.Name;
                item.ButtonClickCommand = new RelayCommand((o) => navigator.SetViewModelOnMain(RegisteredViewModel.OwnerDetails, owner.Id));
                if (owner.ProfilePicture != null) { item.PictureSource = ImageBytes.ConvertBytesToBitmapImage(owner.ProfilePicture.Picture); }
                items.Add(item);
            }
            ItemList = items;
        }
    }
}
