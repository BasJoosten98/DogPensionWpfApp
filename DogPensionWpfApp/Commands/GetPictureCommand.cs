using DogPensionWpfApp.Models.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using DogPensionWpfApp.Models.Database;
using System.IO;
using DogPensionWpfApp.Services;

namespace DogPensionWpfApp.Commands
{
    public class GetPictureCommand : ICommand
    {
        private readonly Action<ProfilePicture, BitmapImage> onSuccess;

        public GetPictureCommand(Action<ProfilePicture, BitmapImage> OnSuccess)
        {
            onSuccess = OnSuccess;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            BitmapImage image = GetImageViaDialog();
            if(image != null)
            {
                ProfilePicture profilePicture = ImageBytes.ConvertBitmapImageToProfilePicture(image);
                onSuccess(profilePicture, image);
                return;
            }
        }

        private BitmapImage GetImageViaDialog()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Choose a picture...";
            dlg.Filter = "All supported formats|*.jpg;*.jpeg;*.png|" +
                        "JPEG|*.jpg;*.jpeg|" +
                        "PNG|*.png";

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                return ImageBytes.GetImageFromFilepath(selectedFileName);
            }
            return null;
        }
    }
}
