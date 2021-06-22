using DogPensionWpfApp.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.Models.Wrappers
{
    public class OwnerWrapper : WrapperBase<Owner>
    {
        public OwnerWrapper(Owner owner)
            : base(owner)
        {

        }

        public Owner Owner { get { return Model; } }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Model.Name)); }
        }
        public string Phonenumber
        {
            get { return Model.Phonenumber; }
            set { Model.Phonenumber = value; OnPropertyChanged(nameof(Model.Phonenumber)); }
        }
        public string Email
        {
            get { return Model.Email; }
            set { Model.Email = value; OnPropertyChanged(nameof(Model.Email)); }
        }
        public string Remarks
        {
            get { return Model.Remarks; }
            set { Model.Remarks = value; OnPropertyChanged(nameof(Model.Remarks)); }
        }

    }
}
