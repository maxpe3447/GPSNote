using Xamarin.Forms;
using Prism;
using Prism.Ioc;
using GPSNote.Services.Repository;
using Xamarin.Essentials;

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
            //await NavigationService.NavigateAsync("NavigationPage/SearchLine");
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<AppInfo, AppInfoImplementation>();

            //Services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());

            //Navigarion
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Views.StartPageView, ViewModels.StartPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.MainPage, ViewModels.MainPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.LogInPageView, ViewModels.LogInPageViewModel>();
            containerRegistry.RegisterForNavigation<Views.MapView, ViewModels.MapViewModel>();
            containerRegistry.RegisterForNavigation<Views.CreateAnAccountView, ViewModels.CreateAnAccountViewModel>();
            containerRegistry.RegisterForNavigation<Views.PinListView, ViewModels.PinListViewModel>();
            containerRegistry.RegisterForNavigation<Views.CreatePinView, ViewModels.CreatePinViewModel>();
        }

    }
}
