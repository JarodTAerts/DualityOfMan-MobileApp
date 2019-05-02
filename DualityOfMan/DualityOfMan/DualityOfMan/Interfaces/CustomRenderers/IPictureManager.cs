using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DualityOfMan.Interfaces.CustomRenderers
{
    public interface IPictureManager
    {
        /// <summary>
        /// Function that will be implemented in both Xamarin.IOS and Xamarin.Droid that will take a byte array and save it as an image in the devices gallery
        /// </summary>
        /// <param name="filename">Name of the file that will be saved</param>
        /// <param name="imageData">Image data in a byte array</param>
        void SavePictureToDisk(string filename, byte[] imageData);

        /// <summary>
        /// Function that will be implemented in both IOS and Android and take a screenshot of a specific view on each respective platform
        /// </summary>
        /// <param name="view">Xamarin Forms view that the screenshot will be taken of</param>
        /// <returns>Byte Array representing the image</returns>
        Task<byte[]> CaptureAsync(View view);
    }
}
