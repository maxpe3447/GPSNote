using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GPSNote.Models;
using GPSNote.Services.Repository;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        public SignUpViewModel(INavigationService navigationService,
                               IRepository repository)
            :base(navigationService)
        {
            Title = "SignUP page";
            Repository = repository;

            SignUpCommand = new Command(SignUpCommandRelease);
        }
        #region -- Properties --
        private string userEmail;
        public string UserEmail
        {
            get => userEmail;
            set => SetProperty(ref userEmail, value);
        }

        private string userPassword;
        public string UserPassword {
            get => userPassword;
            set => SetProperty(ref userPassword, value);
        }

        private string userConfirmPassword;
        public string UserConfirmPassword
        {
            get => userConfirmPassword;
            set => SetProperty(ref userConfirmPassword, value);
        }
        #endregion

        #region -- Command --
        public ICommand SignUpCommand { get; }
        private void SignUpCommandRelease()
        {
            if(!string.IsNullOrEmpty(UserPassword) && UserPassword == UserConfirmPassword && !string.IsNullOrEmpty(UserEmail))
            {
                UserModel userModel = new UserModel()
                {
                    Email = UserEmail,
                    Password = UserPassword
                };
                userModel.Id = Repository.InsertAsync(userModel).Result;

                INavigationParameters keyValues = new NavigationParameters();
                keyValues.Add(nameof(userModel), userModel);

                NavigationService.GoBackAsync(keyValues);
                return;
            }
            Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Pas Un valid");
        }
        #endregion

        #region -- Private -- 
        IRepository Repository { get; }
        #endregion
    }
}