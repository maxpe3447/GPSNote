using GPSNote.Helpers;

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
        public StartPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            LogInCommand = new Command(LogInCommandRelease);
            CreateAnAccountCommand = new Command(CreateAnAccountRelease);

            TextResources = new TextResources(typeof(Resources.TextControls));
        }


        #region -- Properties --
        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set=>SetProperty(ref _textResources, value);   
        }
        #endregion

        #region -- Commands --
        public ICommand LogInCommand { get;}
        public ICommand CreateAnAccountCommand { get;}
        #endregion

        #region -- Private --
        private void LogInCommandRelease()
        {
            NavigationService.NavigateAsync(nameof(Views.LogInPageView));
        }
        private async void CreateAnAccountRelease()
        {
            await NavigationService.NavigateAsync(nameof(CreateAnAccountViewModel));
        }
        #endregion
    }
}
