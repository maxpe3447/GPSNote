using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSNote.ViewModels
{
    class SignInViewModel : ViewModelBase
    {
        public SignInViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Sign in page";
        }
    }
}
