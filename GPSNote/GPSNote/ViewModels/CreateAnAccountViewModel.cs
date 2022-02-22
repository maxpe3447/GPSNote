using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Autherization;
using GPSNote.Services.Repository;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class CreateAnAccountViewModel : ViewModelBase
    {
        public CreateAnAccountViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            TextResources = new TextResources(typeof(Resources.TextControls));

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
            _userModel = new UserModel()
            {
                Email = UserEmail,
                Name = UserName
            };

            INavigationParameters keyValues = new NavigationParameters();
            keyValues.Add(nameof(_userModel), _userModel);

            NavigationService.NavigateAsync(nameof(Views.CreateAccountPassPageView),keyValues);

        }

        public ICommand BackCommand { get; }
        private void BackCommandRelease()
        {
            NavigationService.GoBackAsync();
        }

        #endregion

        #region -- Overrides -- 

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<LinkModel>(nameof(LinkModel), out var link))
            {
                _LinkModel = link;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            if (_LinkModel != null)
            {
                parameters.Add(nameof(LinkModel), _LinkModel);
            }
        }

        #endregion

        #region -- Private -- 
        private UserModel _userModel;
        private LinkModel _LinkModel { get; set; } = null;
        #endregion
    }
}