using GPSNote.Helpers;
using GPSNote.Resources;
using GPSNote.Services.Settings;
using GPSNote.Services.ThemeManager;
using Prism.Navigation;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;

namespace GPSNote.ViewModels
{

    public class SettingsViewModel:ViewModelBase
    {

        readonly private IThemeManager _themeManager;

        public SettingsViewModel(
            INavigationService navigation,
            IThemeManager themeManager)
            :base(navigation)
        {
            _themeManager = themeManager;
            IsDark = Convert.ToBoolean(_themeManager.IsDarkTheme);

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
                _themeManager.IsDarkTheme = Convert.ToInt32(IsDark);
            }
        }
        #endregion

        #region --Command --
        public ICommand BackCommand { get => new DelegateCommand(BackCommandRelease); }

        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}
