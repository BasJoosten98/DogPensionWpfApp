using DogPensionWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.Services
{
    public enum RegisteredViewModel
    {
        Login, Register, OwnerList, DogList, OwnerDetails, DogDetails, ReservationDetails, ReservationList
    }
    public class ViewModelFactory
    {
        private readonly CreateViewModel<LoginViewModel> loginFunc;
        private readonly CreateViewModel<RegisterViewModel> registerFunc;
        private readonly CreateViewModel<OwnerListViewModel> ownerListFunc;
        private readonly CreateViewModel<DogListViewModel> dogListFunc;
        private readonly CreateViewModel<OwnerDetailsViewModel> ownerDetailsFunc;
        private readonly CreateViewModel<DogDetailsViewModel> dogDetailsFunc;
        private readonly CreateViewModel<ReservationDetailsViewModel> reservationDetailsFunc;
        private readonly CreateViewModel<ReservationListViewModel> reservationListFunc;

        public ViewModelFactory(CreateViewModel<LoginViewModel> loginFunc,
            CreateViewModel<RegisterViewModel> registerFunc,
            CreateViewModel<OwnerListViewModel> ownerListFunc,
            CreateViewModel<DogListViewModel> dogListFunc,
            CreateViewModel<OwnerDetailsViewModel> ownerDetailsFunc,
            CreateViewModel<DogDetailsViewModel> dogDetailsFunc,
            CreateViewModel<ReservationDetailsViewModel> reservationDetailsFunc,
            CreateViewModel<ReservationListViewModel> reservationListFunc)
        {
            this.loginFunc = loginFunc;
            this.registerFunc = registerFunc;
            this.ownerListFunc = ownerListFunc;
            this.dogListFunc = dogListFunc;
            this.ownerDetailsFunc = ownerDetailsFunc;
            this.dogDetailsFunc = dogDetailsFunc;
            this.reservationDetailsFunc = reservationDetailsFunc;
            this.reservationListFunc = reservationListFunc;
        }

        public ViewModelBase CreateViewModel(RegisteredViewModel viewModelType)
        {
            switch (viewModelType)
            {
                case RegisteredViewModel.Login:
                    return loginFunc();
                case RegisteredViewModel.Register:
                    return registerFunc();
                case RegisteredViewModel.OwnerList:
                    return ownerListFunc();
                case RegisteredViewModel.DogList:
                    return dogListFunc();
                case RegisteredViewModel.OwnerDetails:
                    return ownerDetailsFunc();
                case RegisteredViewModel.DogDetails:
                    return dogDetailsFunc();
                case RegisteredViewModel.ReservationDetails:
                    return reservationDetailsFunc();
                case RegisteredViewModel.ReservationList:
                    return reservationListFunc();
            }
            throw new Exception("Facory: viewModelType was not added in switch statement");
        }
    }
}