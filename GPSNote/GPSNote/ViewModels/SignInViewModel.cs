using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    class SignInViewModel : ViewModelBase
    {
        public SignInViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Sign in page";

            SigninCommand = new Command(SignInRelease);
        }

        #region -- Command --
        public ICommand SigninCommand { get; }
        private async void SignInRelease()
        {
            await NavigationService.NavigateAsync("/MainPage");
        }
        #endregion

    }
}
