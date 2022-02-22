using GPSNote.Helpers;
using GPSNote.Resources;
using GPSNote.Services.Settings;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class SettingsViewModel:ViewModelBase
    {
        public SettingsViewModel(INavigationService navigation,
                                 ISettingsManager settingsManager)
            :base(navigation)
        {
            _SettingsManager = settingsManager;
            IsDark = _SettingsManager.IsDarkTheme;

            BackCommand = new Command(BackCommandRelease);

            TextResources = new TextResources(typeof(TextControls));
        }

        #region -- Properties --
        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }

        private bool _isDark;
        public bool IsDark
        {
            get => _isDark;
            set
            {
                SetProperty(ref _isDark, value);

                if (IsDark)
                {
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    App.Current.UserAppTheme = OSAppTheme.Light;
                }
                _SettingsManager.IsDarkTheme = IsDark;
            }
        }
        #endregion

        #region --Command --
        public ICommand BackCommand { get; }

        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion

        #region -- Private -- 
        private ISettingsManager _SettingsManager;
        #endregion
    }
}
