using DualityOfMan.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DualityOfMan.ViewModels
{
    /// <summary>
    /// Class that acts as the viewmodel for the MainPage view of the application
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        #region Properties
        // Navigation service used to navigate to other views in the application
        private INavigationService _navigationService { get; set; }

        // String bound to the first textbox on the main page view
        private string _duality1;
        public string Duality1
        {
            get { return _duality1; }
            set { SetProperty(ref _duality1, value); }
        }

        // String bound to the second textbox on the main page view
        private string _duality2;
        public string Duality2
        {
            get { return _duality2; }
            set { SetProperty(ref _duality2, value); }
        }

        #endregion

        #region Commands
        /// <summary>
        /// Command which links the Create Custom Man button with a function defined in this viewmodel
        /// </summary>
        public ICommand CreateManCommand { get; set; }

        /// <summary>
        /// Command which links the settings navbar button with a function defined in this viewmodel
        /// </summary>
        public ICommand SettingsCommand { get; set; }

        /// <summary>
        /// Command which links the Create Random Man button with a function defined in this viewmodel
        /// </summary>
        public ICommand CreateRandomManCommand { get; set; }

        #endregion

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Duality of Man";
            
            // Connect functions in this view model to the commands that are bound to the view
            CreateManCommand = new Command(CreateManClicked);
            SettingsCommand = new Command(Settings);
            CreateRandomManCommand = new Command(CreateRandomManClicked);
        }

        #region Command Functions
        /// <summary>
        /// Function that will grab the values from the properties bound to the text boxes and send them to a function to create a duality of man meme
        /// </summary>
        private void CreateManClicked()
        {
            CreateMan(Duality1, Duality2);
        }

        /// <summary>
        /// Function that gets two random words and then sends them to the function to create a duality of man meme
        /// </summary>
        private void CreateRandomManClicked()
        {
            // Get Random Words from the data container
            string duality1 = DataContainer.GetRandomWord();
            string duality2 = DataContainer.GetRandomWord();

            CreateMan(duality1, duality2);
        }

        /// <summary>
        /// Function to navigate to the settings/about page 
        /// </summary>
        private async void Settings()
        {
            await _navigationService.NavigateAsync("NavigationPage/SettingsPage", useModalNavigation: true);
        }

        #endregion

        #region Helper Functions
        /// <summary>
        /// Function that takes two strings and sends them to the DualityViewerPage to be shown as a duality of man meme
        /// </summary>
        /// <param name="duality1"></param>
        /// <param name="duality2"></param>
        private async void CreateMan(string duality1, string duality2)
        {
            // Create the naviation parameters and add the two duality strings to it
            var navigationParams = new NavigationParameters();
            navigationParams.Add("DUALITIES", new List<string> { duality1, duality2 });
            
            // Navigate to DualityViewerPage with the navparams and using modal navigation 
            await _navigationService.NavigateAsync("NavigationPage/DualityViewerPage", navigationParams, useModalNavigation: true);
        }

        #endregion

    }
}
