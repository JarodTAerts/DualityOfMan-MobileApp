using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DualityOfMan.Interfaces.CustomRenderers;
using DualityOfMan.iOS.CustomRenderers;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(PictureManager_IOS))]
namespace DualityOfMan.iOS.CustomRenderers
{
    /// <summary>
    /// Class that implements the IPictureManager interface for IOS
    /// </summary>
    public class PictureManager_IOS : IPictureManager
    {
        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            var saveImage = new UIImage(NSData.FromArray(imageData));
            saveImage.SaveToPhotosAlbum((image, error) =>
            {
                //you can retrieve the saved UI Image as well if needed using  
                //var i = image as UIImage;  
                if (error != null)
                {
                    Console.WriteLine(error.ToString());
                }
            });
        }

        public async Task<byte[]> CaptureAsync(View view)
        {
            var nativeView = Xamarin.Forms.Platform.iOS.Platform.GetRenderer(view).NativeView;
            UIGraphics.BeginImageContextWithOptions(nativeView.Bounds.Size, opaque: true, scale: 0);
            nativeView.DrawViewHierarchy(nativeView.Bounds, afterScreenUpdates: true);
            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            using (var imageData = image.AsPNG())
            {
                var bytes = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, bytes, 0, Convert.ToInt32(imageData.Length));
                await Task.Delay(100);
                return bytes;
            }
        }
    }
}