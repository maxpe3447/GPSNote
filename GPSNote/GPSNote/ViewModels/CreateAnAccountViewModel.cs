using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
using GPSNote.Services.Repository;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class CreateAnAccountViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private LinkModel _LinkModel { get; set; } = null;
        private IAuthentication _authentication;

        public CreateAnAccountViewModel(INavigationService navigationService,
                                        IAuthentication authentication)
            :base(navigationService)
        {
            _authentication = authentication;

            TextResources = new TextResources(typeof(Resources.TextControls));

            nextCommand = new DelegateCommand(NextCommandRelease);
            backCommand = new DelegateCommand(BackCommandRelease);
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

        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }

        private Color _errorColor;
        public Color ErrorColor
        {
            get => _errorColor;
            set => SetProperty(ref _errorColor, value);
        }

        private string _passwordErrorMsgText;
        public string EmailErrorMsgText
        {
            get => _passwordErrorMsgText;
            set => SetProperty(ref _passwordErrorMsgText, value);
        }

        private ICommand nextCommand;
        public ICommand NextCommand { get => nextCommand ?? new DelegateCommand(NextCommandRelease); }

        private ICommand backCommand;
        public ICommand BackCommand { get => backCommand ?? new DelegateCommand(BackCommandRelease); }

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
        private void NextCommandRelease()
        {
            if (_authentication.IsExistEmail(UserEmail))
            {
                ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightRed];
                EmailErrorMsgText = Resources.UserMsg.EmailExist;
                return;
            }

            if (!string.IsNullOrEmpty(EmailErrorMsgText))
            {
                EmailErrorMsgText = string.Empty;
                ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightGray];
            }

            _userModel = new UserModel()
            {
                Email = UserEmail,
                Name = UserName
            };

            INavigationParameters keyValues = new NavigationParameters();
            keyValues.Add(nameof(_userModel), _userModel);

            NavigationService.NavigateAsync($"/{nameof(Views.CreateAccountPassPageView)}", keyValues);
        }

        private void BackCommandRelease()
        {
            NavigationService.NavigateAsync($"/{nameof(Views.StartPageView)}");
        }
        #endregion
    }
}