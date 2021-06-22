using DogPensionWpfApp.Models.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogPensionWpfApp.Models.Wrappers
{
    public class DogWrapper : WrapperBase<Dog>
    {
        public DogWrapper(Dog dog) : base(dog)
        {

        }

        public Dog Dog { get { return Model; } }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(nameof(Model.Name)); }
        }
        public string Breed
        {
            get { return Model.Breed; }
            set { Model.Breed = value; OnPropertyChanged(nameof(Model.Breed)); }
        }
        public string Remarks
        {
            get { return Model.Remarks; }
            set { Model.Remarks = value; OnPropertyChanged(nameof(Model.Remarks)); }
        }

    }
}
