using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Repository;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class CreateAnAccountViewModel : ViewModelBase
    {
        public CreateAnAccountViewModel(INavigationService navigationService,
                                        IRepository repository)
            :base(navigationService)
        {
            TextResources = new TextResources(typeof(Resources.TextControls));

            _Repository = repository;

            NextCommand = new Command(NextCommandRelease);
            BackCommand = new Command(BackCommandRelease);
        }
        #region -- Properties --
        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        //private string userPassword;
        //public string UserPassword {
        //    get => userPassword;
        //    set => SetProperty(ref userPassword, value);
        //}

        //private string userConfirmPassword;
        //public string UserConfirmPassword
        //{
        //    get => userConfirmPassword;
        //    set => SetProperty(ref userConfirmPassword, value);
        //}

        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }
        #endregion

        #region -- Command --
        public ICommand NextCommand { get; }
        private void NextCommandRelease()
        {
            UserModel userModel = new UserModel()
            {
                Email = UserEmail,
                Name = UserName
            };

            //userModel.Id = _Repository.InsertAsync(userModel).Result;

            INavigationParameters keyValues = new NavigationParameters();
            keyValues.Add(nameof(UserModel), userModel);

            NavigationService.NavigateAsync(nameof(Views.CreateAccountPassPageView),keyValues);

        }

        public ICommand BackCommand { get; }
        private void BackCommandRelease()
        {
            NavigationService.GoBackAsync();
        }

        #endregion

        #region -- Private -- 
        private IRepository _Repository { get; }
        #endregion
    }
}