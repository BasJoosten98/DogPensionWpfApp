using DogPensionWpfApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DogPensionWpfApp.Models
{
    public class OwnerDogListItem
    {
        public string Name { get; set; } = "Unnamed";
        public BitmapImage PictureSource { get; set; } = new BitmapImage(new Uri("pack://application:,,,/Resources/no_image.jpg"));
        public string ButtonContent { get; set; } = "Show more";
        public ICommand ButtonClickCommand { get; set; }
    }
}
