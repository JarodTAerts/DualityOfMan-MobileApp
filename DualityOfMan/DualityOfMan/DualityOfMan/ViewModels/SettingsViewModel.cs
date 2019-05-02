using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DualityOfMan.ViewModels
{
    class SettingsViewModel : ViewModelBase
    {
        #region Properties

        // Navigation service used to navigate to other views in the application
        INavigationService _navigationService;

        #endregion

        #region Command Functions

        /// <summary>
        /// Command which links and binds the back navbar button in the view to a function in this view model
        /// </summary>
        public ICommand BackCommand { get; set; }

        /// <summary>
        /// Command which links and binds the Open Website button in the view to a function in this view models
        /// </summary>
        public ICommand OpenWebsiteCommand { get; set; }

        #endregion

        public SettingsViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Settings";
            _navigationService = navigationService;

            // Register the commands
            BackCommand = new Command(Back);
            OpenWebsiteCommand = new Command(OpenWebsite);
        }

        #region Command Functions

        /// <summary>
        /// Function to navigate back to the page that invoked this page
        /// </summary>
        private async void Back()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true);
        }

        /// <summary>
        /// Function to open the duality of man website in an external browser
        /// </summary>
        private void OpenWebsite()
        {
            Device.OpenUri(new Uri("http://www.dualityofman.pw"));
        }

        #endregion
    }
}
