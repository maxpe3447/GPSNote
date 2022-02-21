using GPSNote.Helpers;
using GPSNote.Resources;
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
        public SettingsViewModel(INavigationService navigation):base(navigation)
        {
            BackCommand = new Command(BackCommandRelease);
            ThemeChangeCommand = new Command(ThemeChangeCommandRelease);

            TextResources = new TextResources(typeof(TextControls));
        }

        #region -- Properties --
        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }
        #endregion

        #region --Command --
        public ICommand BackCommand { get; }

        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        public ICommand ThemeChangeCommand { get; }

        private void ThemeChangeCommandRelease()
        {
            //if(App.Current.UserAppTheme == OSAppTheme.Light)
            //{
            //    App.Current.UserAppTheme = OSAppTheme.Dark;
            //}
            //else if(App.Current.UserAppTheme == OSAppTheme.Dark)
            //{
            //    App.Current.UserAppTheme = OSAppTheme.Light;
            //}

            App.Current.Resources["BackGround"] = App.Current.Resources["Black"];
        }
        #endregion
    }
}
