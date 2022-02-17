using Xamarin.Forms;
using Prism;
using Prism.Ioc;
using GPSNote.Services.Repository;
using Xamarin.Essentials;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
using GPSNote.Services.Settings;

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
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<AppInfo, AppInfoImplementation>();

            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<IAuthentication>(Container.Resolve<Authentication>());
            containerRegistry.RegisterInstance<IAutherization>(Container.Resolve<Autherization>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());

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
        }

    }
}
