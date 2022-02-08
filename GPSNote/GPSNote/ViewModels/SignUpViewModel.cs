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
            _Repository = repository;

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
            UserModel userModel = new UserModel()
            {
                Email = UserEmail,
                Password = UserPassword
            };

            if (string.IsNullOrEmpty(UserPassword) && UserPassword != UserConfirmPassword && string.IsNullOrEmpty(UserEmail) && _Repository.IsExistAsync(userModel, out int id))
            {
                Acr.UserDialogs.UserDialogs.Instance.AlertAsync("Password or Email invalid or User is exist");
                return;
            }
            userModel.Id = _Repository.InsertAsync(userModel).Result;

            INavigationParameters keyValues = new NavigationParameters();
            keyValues.Add(nameof(UserModel), userModel);

            NavigationService.GoBackAsync(keyValues);

        }
        #endregion

        #region -- Private -- 
        private IRepository _Repository { get; }
        #endregion
    }
}