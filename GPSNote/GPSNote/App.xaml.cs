using Xamarin.Forms;
using Prism;
using Prism.Ioc;
using GPSNote.Services.Repository;
using Xamarin.Essentials;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
using GPSNote.Services.Settings;
using GPSNote.Services.PinManager;
using System;
using Prism.Navigation;
using GPSNote.Models;
using GPSNote.Services.ThemeManager;
using GPSNote.Services.LinkManager;

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
            await NavigationService.NavigateAsync(nameof(Views.StartPageView));
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            const char SEPARATOR = '/';

            if(uri.Host.ToLower() == $"{nameof(GPSNote)}.{nameof(App)}".ToLower() && uri.Segments != null && uri.Segments.Length == 6)
            {
                string action = uri.Segments[1].Trim(SEPARATOR);
                bool isActionLatValid = double.TryParse(uri.Segments[2].Trim(SEPARATOR), out double LatLatitude);
                bool isActionLongValid = double.TryParse(uri.Segments[3].Trim(SEPARATOR), out double Longitude);
                
                if (action.ToLower() == "geo" && isActionLatValid && isActionLongValid)
                {
                    NavigationParameters parameters = new NavigationParameters();
                    if(LatLatitude > 0 && Longitude > 0)
                    {
                        LinkModel linkModel = new LinkModel
                        {
                            Latitude = LatLatitude,
                            Longitude = Longitude,
                            Name = uri.Segments[4].Trim(SEPARATOR),
                            Description = uri.Segments[5]

                        };
                        parameters.Add(nameof(LinkModel), linkModel);
                    }
                    NavigationService.NavigateAsync(nameof(Views.StartPageView), parameters);
                }
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<AppInfo, AppInfoImplementation>();

            //Services
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthentication>(Container.Resolve<Authentication>());
            containerRegistry.RegisterInstance<IAutherization>(Container.Resolve<Autherization>());
            containerRegistry.RegisterInstance<IPinManager>(Container.Resolve<PinManager>());
            containerRegistry.RegisterInstance<IThemeManager>(Container.Resolve<ThemeManager>());
            containerRegistry.RegisterInstance<ILinkManager>(Container.Resolve<LinkManager>());

            //Navigarion
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.StartPageView, ViewModels.StartPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.LogInPageView, ViewModels.LogInPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.MainPage>();//, ViewModels.MainPageViewModel
            containerRegistry.RegisterForNavigation<Views.MapView, ViewModels.MapViewModel>();
            containerRegistry.RegisterForNavigation<Views.CreateAnAccountView, ViewModels.CreateAnAccountViewModel>();
            containerRegistry.RegisterForNavigation<Views.PinListView, ViewModels.PinListViewModel>();
            containerRegistry.RegisterForNavigation<Views.CreatePinView, ViewModels.CreatePinViewModel>();
            containerRegistry.RegisterForNavigation<Views.CreateAccountPassPageView, ViewModels.CreateAccountPassPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.SettingsView, ViewModels.SettingsViewModel>();
        }

    }
}
