using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Xamarin.Essentials.Implementation;
//using Xamarin.Essentials.Interfaces;
using Prism;
using Prism.Navigation;
using Prism.Ioc;


namespace GPSNote
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            :base(initializer)
        {
            

            //MainPage = new MainPage();
           
        }

        
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<IAppInfo>

            //
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MainPage>();
        }

    }
}
