using Prism;
using Prism.Ioc;
using DualityOfMan.ViewModels;
using DualityOfMan.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using System.Reflection;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
using DualityOfMan.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DualityOfMan
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }


        protected override async void OnInitialized()
        {
            InitializeComponent();

            ReadWordsFromLocalJson();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        /// <summary>
        /// Function that registers the DI componenets for the application
        /// </summary>
        /// <param name="containerRegistry">DI contatiner that the interfaces are registered in</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<DualityViewerPage, DualityViewerViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsViewModel>();
        }

        /// <summary>
        /// Function to open the words json file and read all the words into a list which is held in the DataContainer
        /// so that it can be used across the application
        /// </summary>
        private void ReadWordsFromLocalJson()
        {
            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream("DualityOfMan.Assets.words_dictionary.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    // TODO: Make this process faster. Possibly use the basic text file instead of JSON and JObject
                    string jsonString = reader.ReadToEnd();
                    JObject jsonObject = JObject.Parse(jsonString);
                    DataContainer.WordData = jsonObject.Properties().Select(p => p.Name).ToList();
                }
            }
        }

    }
}
