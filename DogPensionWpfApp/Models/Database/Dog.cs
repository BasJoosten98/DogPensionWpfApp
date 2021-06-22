using DogPensionWpfApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DogPensionWpfApp.Models.Database
{
    public class Dog : WrapperModelBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Breed { get; set; }

        public string  Remarks { get; set; }

        [Required]
        public int OwnerId { get; set; }

        public int? ProfilePictureId { get; set; }

        public virtual Owner Owner { get; set; }

        public virtual ProfilePicture ProfilePicture { get; set; }
    }
}
