using DogPensionWpfApp.Commands;
using DogPensionWpfApp.Extensions;
using DogPensionWpfApp.Models.Database;
using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace DogPensionWpfApp.ViewModels
{
    public class MainViewModel : ViewModelBase, IViewHolder
    {
        private ViewModelBase _currentViewModel;
        private readonly Authenticator authenticator;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand LogoutCommand { get; }
        public ICommand MainNavigateCommand { get; }

        public bool IsLoggedIn => authenticator.Account != null;

        public MainViewModel(Navigator navigator, Authenticator authenticator)
        {
            LogoutCommand = new LogoutCommand(authenticator, navigator);
            MainNavigateCommand = new MainNavigateCommand(navigator);

            authenticator.Account_Changed += Authenticator_Account_Changed;

            navigator.AddViewHolder(this);
            navigator.SetViewModelOnMain(RegisteredViewModel.Login);

            this.authenticator = authenticator;
            //authenticator.Login("Bas Joosten", "pass1234".ToSecureString());
        }

        private void Authenticator_Account_Changed()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        public override void Load(object arguments)
        {
            //No loading needed
        }
    }
}
