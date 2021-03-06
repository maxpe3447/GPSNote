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
        readonly private IThemeManagerService _themeManager;

        public SettingsViewModel(
            INavigationService navigation,
            IThemeManagerService themeManager)
            :base(navigation)
        {
            _themeManager = themeManager;
            IsDark = _themeManager.IsDarkTheme;

            TextResources = new TextResources(typeof(TextControls));
        }

        #region -- Properties --
        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }

        private int _isDark;
        public int IsDark
        {
            get => _isDark;
            set => SetProperty(ref _isDark, value, ActionSetPropertyIsDark);
        }

        private ICommand backCommand;
        public ICommand BackCommand { get => backCommand ??= new DelegateCommand(BackCommandRelease); }
        #endregion

        #region -- Private -- 
        private async void BackCommandRelease()
            => await NavigationService.GoBackAsync();

        private void ActionSetPropertyIsDark()
        {
            if (IsDark != default(int))
            {
                App.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                App.Current.UserAppTheme = OSAppTheme.Light;
            }
            _themeManager.IsDarkTheme = IsDark;
        }
        #endregion
    }
}
