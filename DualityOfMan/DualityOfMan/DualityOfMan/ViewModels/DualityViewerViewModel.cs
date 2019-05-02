using Acr.UserDialogs;
using DualityOfMan.Interfaces.CustomRenderers;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DualityOfMan.ViewModels
{
    public class DualityViewerViewModel : ViewModelBase, INavigationAware
    {
        #region Properties
        // Navigation service used to navigate to other views in the application
        INavigationService _navigationService;

        // String bound to the labels on the left duality disks on the image
        private string _duality1;
        public string Duality1
        {
            get { return _duality1; }
            set { SetProperty(ref _duality1, value); }
        }

        // String bound to the labels on the right duality disks on the image
        private string _duality2;
        public string Duality2
        {
            get { return _duality2; }
            set { SetProperty(ref _duality2, value); }
        }

        // Boolean to bound to the loading indicator to show when activity is happening in the app
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        #endregion

        #region Commands
        
        /// <summary>
        /// Command which links and binds the cancel navbar button to a function implemented in this view model
        /// </summary>
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Command which links and binds the save button in the view to a function in this view model
        /// </summary>
        public ICommand SaveCommand { get; set; }
        
        #endregion

        public DualityViewerViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "View";
            _navigationService = navigationService;
            IsLoading = false;
            CancelCommand = new Command(Cancel);
            SaveCommand = new Command<AbsoluteLayout>(Save);
        }

        #region Command Functions

        /// <summary>
        /// Function that saves a picture of a specific view
        /// </summary>
        /// <param name="view">View which a picture will be saved of</param>
        private async void Save(View view)
        {
            // Show the activity monitor to the user
            IsLoading = true;

            // Start task on other thread to take a screenshot of the view passed into this function and save it to gallery
            await Task.Run(() => TakeAndSaveScreenShot(view));

            // Show alert to user so they know the image was saved successfully
            await UserDialogs.Instance.AlertAsync("Image was successfully saved to gallery.","Image Saved!", "Cool!");
        }

        /// <summary>
        /// Function to go back to the page that invoked this page
        /// </summary>
        private async void Cancel()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true);
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Function to take a view and call a customer renderer to take a screenshot of that view and save it to gallery
        /// </summary>
        /// <param name="view">View that screenshot will be taken of</param>
        /// <returns>Task with nothing in it</returns>
        private async Task TakeAndSaveScreenShot(View view)
        {
            // Get screenshotManager from dependency service
            IPictureManager pictureManager = DependencyService.Get<IPictureManager>();

            //  Actually take the screenshot and get back a byte[] of data
            var screenshotData = await pictureManager.CaptureAsync(view);

            // Save Duality of Man image to gallery
            pictureManager.SavePictureToDisk("DualityOfManImage", screenshotData);

            // Stop activity indiactor from showing
            IsLoading = false;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Function to run when the page is navigated to and read the duality1 and 2 objects from the navigation parameters
        /// </summary>
        /// <param name="parameters">Navigation Parameters that were sent from the invoking view</param>
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            List<string> dualities = parameters.GetValue<List<string>>("DUALITIES");

            // Get the dualities from the list sent in as long as there are objects
            Duality1 = dualities[0] != null ? dualities[0] : "";
            Duality2 = dualities[1] != null ? dualities[1] : "";
        }

        #endregion

    }
}
