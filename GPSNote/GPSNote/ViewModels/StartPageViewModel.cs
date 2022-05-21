using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
using GPSNote.Services.LinkManager;
using GPSNote.Services.Settings;
using GPSNote.Services.ThemeManager;
using GPSNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {

        #region -- Private --
        readonly private IAuthentication _authentication;
        readonly private IThemeManager _themeManager;
        readonly private ILinkManager _linkManager;

        private void LogInCommandRelease()
        {
            NavigationService.NavigateAsync($"/{nameof(LogInPageView)}");
        }
        private async void CreateAnAccountRelease()
        {
            await NavigationService.NavigateAsync($"/{nameof(CreateAnAccountView)}");
        }
        #endregion

        public StartPageViewModel(
            INavigationService navigationService,
            IAuthentication authentication,
            IThemeManager themeManager,
            ILinkManager linkManager
            ) : base(navigationService)
        {
            _authentication = authentication;
            _themeManager = themeManager;
            _linkManager = linkManager;

            TextResources = new TextResources(typeof(Resources.TextControls));
        }


        #region -- Properties --
        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }
        #endregion

        #region -- Commands --
        public ICommand LogInCommand
        {
            get => new DelegateCommand(LogInCommandRelease);
        }
        public ICommand CreateAnAccountCommand 
        {
            get => new DelegateCommand(CreateAnAccountRelease);
        }
        #endregion

        #region -- Overrides --
        public override void Initialize(INavigationParameters parameters)
        {
            try
            {
                if (_themeManager.IsDarkTheme != default(int))
                {
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                }
            }catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.AlertAsync(ex.Message);
            }
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.TryGetValue<LinkModel>(nameof(LinkModel), out var link))
            {
                _linkManager.SetLinkModel(link);
            }
            if (_authentication.UserId != default(int))
            {
                await NavigationService.NavigateAsync($"/{nameof(MainPage)}?createTab={nameof(MapView)}&createTab={nameof(PinListView)}");
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {

        }
        #endregion
    }
}
