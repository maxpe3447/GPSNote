﻿using GPSNote.Models;
using GPSNote.Services.Repository;
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
        public SignInViewModel(INavigationService navigationService,
                               IRepository repository) 
            : base(navigationService)
        {
            Title = "Sign in page";
            Repository = repository;

            SigninCommand = new Command(SignInRelease);
            SignUpCommand = new Command(SignUpRelease);
        }

        #region -- Properties -- 
        private string userEmail;
        public string UserEmail
        {
            get => userEmail;
            set => SetProperty(ref userEmail, value);
        }

        private string userPassword;
        public string UserPassword
        {
            get => userPassword;
            set => SetProperty(ref userPassword, value);
        }
        #endregion

        #region -- Command --
        public ICommand SigninCommand { get; }
        private async void SignInRelease()
        {
            if(userModel == null)
            {
                userModel = new UserModel
                {
                    Email = UserEmail,
                    Password = UserPassword
                };
            }

            if (!string.IsNullOrEmpty(UserPassword) && !string.IsNullOrEmpty(UserEmail) && Repository.IsExistAsync(userModel).Result)
                await NavigationService.NavigateAsync("/MainPage");
            else
                Acr.UserDialogs.UserDialogs.Instance.AlertAsync($"oops(((");
        }

        public ICommand SignUpCommand { get; }
        private async void SignUpRelease()
        {
            await NavigationService.NavigateAsync("SignUpView");
        }
        #endregion

        #region -- Override --
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("userModel"))
            {
                userModel = parameters.GetValue<UserModel>("userModel");

            }
        }
        #endregion

        #region -- Private --
        UserModel userModel = null;
        IRepository Repository { get; }
        #endregion
    }
}
