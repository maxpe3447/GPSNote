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
            
        }

        
        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationPage/SignInView");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<IAppInfo>

            //
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.MainPage>();
            containerRegistry.RegisterForNavigation<Views.SignInView, ViewModels.SignInViewModel>();
            containerRegistry.RegisterForNavigation<Views.MapView, ViewModels.MapViewModal>();
        }

    }
}
