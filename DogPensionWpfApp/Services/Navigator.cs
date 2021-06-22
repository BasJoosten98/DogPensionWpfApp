using DogPensionWpfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.Services
{
    public interface IViewHolder
    {
        public ViewModelBase CurrentViewModel { get; set; }
    }
    public class Navigator
    {
        private readonly ViewModelFactory factory;
        private readonly List<IViewHolder> holders;
        private IViewHolder mainViewModel;

        public Navigator(ViewModelFactory factory)
        {
            holders = new List<IViewHolder>();
            this.factory = factory;
        }

        public bool AddViewHolder(IViewHolder holder)
        {
            if (!holders.Contains(holder))
            {
                if(holder is MainViewModel){ mainViewModel = holder; }
                holders.Add(holder);
                return true;
            }
            return false;
        }

        public bool RemoveViewHolder(IViewHolder holder)
        {
            if (holders.Contains(holder))
            {
                holders.Remove(holder);
                return true;
            }
            return false;
        }

        #region SetView
        public bool SetViewModelOnMain(ViewModelBase newViewModel, object arguments = null)
        {
            return setViewModelOnHolder(mainViewModel, newViewModel, arguments);
        }
        public bool SetViewModelOnMain(RegisteredViewModel newViewModelType, object arguments = null)
        {
            ViewModelBase newViewModel = factory.CreateViewModel(newViewModelType);
            return setViewModelOnHolder(mainViewModel, newViewModel, arguments);
        }
        public bool SetViewModelAsHolder(IViewHolder me, ViewModelBase newViewModel, object arguments = null)
        {
            return setViewModelOnHolder(me, newViewModel, arguments);
        }
        public bool SetViewModelAsHolder(IViewHolder me, RegisteredViewModel newViewModelType, object arguments = null)
        {
            ViewModelBase newViewModel = factory.CreateViewModel(newViewModelType);
            return setViewModelOnHolder(me, newViewModel, arguments);
        }
        public bool SetViewModel(ViewModelBase curViewModel, ViewModelBase newViewModel, object arguments = null)
        {
            //PROBLEEM, WAT ALS EEN IVIEWHOLDER DIT CALLED MET curViewModel == NULL????
            IViewHolder holder = holders.Find(h => h.CurrentViewModel == curViewModel);
            return setViewModelOnHolder(holder, newViewModel, arguments);
        }
        public bool SetViewModel(ViewModelBase curViewModel, RegisteredViewModel newViewModelType, object arguments = null)
        {
            IViewHolder holder = holders.Find(h => h.CurrentViewModel == curViewModel);
            ViewModelBase newViewModel = factory.CreateViewModel(newViewModelType);
            return setViewModelOnHolder(holder, newViewModel, arguments);
        }
        private bool setViewModelOnHolder(IViewHolder holder, ViewModelBase newViewModel, object arguments = null)
        {
            if (holder != null)
            {
                newViewModel?.Load(arguments); 
                holder.CurrentViewModel = newViewModel;
                return true;
            }
            return false;
        }
        #endregion
    }
}
