using Xamarin.Forms;
using Prism;
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
using Prism.Ioc;
using GPSNote.Helpers;
using GPSNote.Services.Weather;

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

            if (uri.Host.ToLower() == Constants.LINK_DOMEN.ToLower() && uri.Segments != null && uri.Segments.Length >= 5)
            {
                string action = uri.Segments[1].Trim(Constants.LINK_SEPARATOR[0]);
                bool isActionLatValid = double.TryParse(uri.Segments[2].Trim(Constants.LINK_SEPARATOR[0]), out double LatLatitude);
                bool isActionLongValid = double.TryParse(uri.Segments[3].Trim(Constants.LINK_SEPARATOR[0]), out double Longitude);

                if (action.ToLower() == Constants.LINK_GEO && isActionLatValid && isActionLongValid)
                {
                    NavigationParameters parameters = new NavigationParameters();
                    if(LatLatitude > 0 && Longitude > 0)
                    {
                        LinkModel linkModel = new LinkModel
                        {
                            Latitude = LatLatitude,
                            Longitude = Longitude,
                            Name = uri.Segments[4].Trim(Constants.LINK_SEPARATOR[0]),
                            Description = (uri.Segments.Length == Constants.LINK_MAX_COUNT_SECTION)? uri.Segments[5]: string.Empty

                        };
                        parameters.Add(nameof(LinkModel), linkModel);
                    }
                    NavigationService.NavigateAsync($"/{nameof(Views.StartPageView)}", parameters);
                }
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<AppInfo, AppInfoImplementation>();

            //Services
            containerRegistry.RegisterInstance<ISettingsManagerService>(Container.Resolve<SettingsManagerService>());
            containerRegistry.RegisterInstance<IRepositoryService>(Container.Resolve<RepositoryService>());
            containerRegistry.RegisterInstance<IAuthenticationService>(Container.Resolve<AuthenticationService>());
            containerRegistry.RegisterInstance<IAutherizationService>(Container.Resolve<AutherizationService>());
            containerRegistry.RegisterInstance<IPinManagerService>(Container.Resolve<PinManagerService>());
            containerRegistry.RegisterInstance<IThemeManagerService>(Container.Resolve<ThemeManagerService>());
            containerRegistry.RegisterInstance<ILinkManagerService>(Container.Resolve<LinkManagerService>());
            containerRegistry.RegisterInstance<IWeatherService>(Container.Resolve<WeatherService>());

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
