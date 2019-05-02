using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DualityOfMan.Droid.CustomRenderers;
using DualityOfMan.Interfaces.CustomRenderers;
using Java.IO;
using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.Dependency(typeof(PictureManager_Android))]
namespace DualityOfMan.Droid.CustomRenderers
{
    /// <summary>
    /// Class that implements the IPictureManager Interface for android
    /// </summary>
    public class PictureManager_Android : IPictureManager
    {
        private static Activity _currentActivity;
        public static void SetActivity(Activity activity) => _currentActivity = activity;

        public void SavePictureToDisk(string filename, byte[] imageData)
        {
            // Get file path where the image will be saved on the device
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            var pictures = dir.AbsolutePath;

            //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name  
            // Then create the final full filePath
            string name = filename + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            string filePath = System.IO.Path.Combine(pictures, name);
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                //mediascan adds the saved image into the gallery  
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
                Application.Context.SendBroadcast(mediaScanIntent);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

        }

        public async Task<byte[]> CaptureAsync(Xamarin.Forms.View view)
        {
            if (_currentActivity == null)
            {

                throw new Exception("You have to set ScreenshotManager.Activity in your Android project");
            }

            var nativeView = view.GetRenderer().View;
            var wasDrawingCacheEnabled = nativeView.DrawingCacheEnabled;
            nativeView.DrawingCacheEnabled = true;
            nativeView.BuildDrawingCache(false);

            // Actually take the screenshot
            Bitmap bitmap = nativeView.GetDrawingCache(false);

            byte[] bitmapData;

            // Put the bitmap into a dataform that we are able to use and process 
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            // Reset the view drawingCacheEnabled to what it was before we took the screenshot
            nativeView.DrawingCacheEnabled = wasDrawingCacheEnabled;

            await Task.Delay(1);
            return bitmapData;
        }
    }
}