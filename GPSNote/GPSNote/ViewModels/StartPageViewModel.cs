using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Settings;
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
        public StartPageViewModel(INavigationService navigationService,
                                  ISettingsManager settingsManager) : base(navigationService)
        {
            _SettingsManager = settingsManager;

            LogInCommand = new Command(LogInCommandRelease);
            CreateAnAccountCommand = new Command(CreateAnAccountRelease);

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
        public ICommand LogInCommand { get; }
        public ICommand CreateAnAccountCommand { get; }
        #endregion

        #region -- Overrides --
        public override void Initialize(INavigationParameters parameters)
        {
            if (_SettingsManager.IsDarkTheme)
            {
                App.Current.UserAppTheme = OSAppTheme.Dark;
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue<LinkModel>(nameof(LinkModel), out var link))
            {
                
                _LinkModel = link;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if(_LinkModel != null)
            {
                parameters.Add(nameof(LinkModel), _LinkModel);
            }
        }
        #endregion

        #region -- Private --
        private void LogInCommandRelease()
        {
            NavigationService.NavigateAsync(nameof(Views.LogInPageView));
        }
        private async void CreateAnAccountRelease()
        {
            await NavigationService.NavigateAsync(nameof(Views.CreateAnAccountView));
        }

        private ISettingsManager _SettingsManager { get; }
        private LinkModel _LinkModel { get; set; } = null;
        #endregion
    }
}
