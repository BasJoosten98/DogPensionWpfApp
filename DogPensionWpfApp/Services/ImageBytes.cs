using DogPensionWpfApp.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace DogPensionWpfApp.Services
{
    public static class ImageBytes
    {
        public static ProfilePicture ConvertBitmapImageToProfilePicture(BitmapImage bitmapImage)
        {
            string[] splittedUri = bitmapImage.UriSource.OriginalString.Split('.');
            string fileType = splittedUri[splittedUri.Length - 1].ToLower();
            ProfilePicture profilePicture = new ProfilePicture
            {
                FileName = Guid.NewGuid().ToString(),
                FileType = fileType,
                Picture = ConvertBitmapImageToBytes(bitmapImage, fileType)
            };
            return profilePicture;
        }
        public static BitmapImage GetImageFromFilepath(string filepath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filepath);
            bitmap.EndInit();
            return bitmap;
        }
        public static BitmapImage ConvertBytesToBitmapImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public static byte[] ConvertBitmapImageToBytes(BitmapImage bitmapImage, string fileType)
        {
            byte[] data;
            BitmapEncoder encoder = getEncoder(fileType);
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        private static BitmapEncoder getEncoder(string fileType)
        {
            switch (fileType)
            {
                case "jpg":
                    return new JpegBitmapEncoder();
                case "jpeg":
                    return new JpegBitmapEncoder();
                case "png":
                    return new PngBitmapEncoder();
                default:
                    throw new Exception("SetPicture: Encoder not implemented in switch statement");
            }
        }
    }
}
