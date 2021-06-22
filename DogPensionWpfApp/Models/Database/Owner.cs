using DogPensionWpfApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DogPensionWpfApp.Models.Database
{
    public class Owner : WrapperModelBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phonenumber { get; set; }

        public string Remarks { get; set; }

        [Required]
        public int AccountId { get; set; }

        public int? ProfilePictureId { get; set; }

        public virtual ICollection<Dog> Dogs { get; set; }

        public virtual Account Account { get; set; }

        public virtual ProfilePicture ProfilePicture { get; set; }
    }
}
