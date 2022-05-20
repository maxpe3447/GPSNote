using Acr.UserDialogs;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
using GPSNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{

    public class CreateAccountPassPageViewModel : ViewModelBase
    {
        #region Private
        readonly private IAutherization _autherization;
        readonly private IAuthentication _authentication;

        private UserModel _userModel;
        private LinkModel _LinkModel { get; set; } = null;

        private async void BackCommandRelease()
        {
            await NavigationService.NavigateAsync($"/{nameof(CreateAnAccountView)}");
        }
        private async void CreateAccountCommandRelease()
        {
            if (UserPassword != UserPasswordRepeat)
            {
                ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightRed];
                PasswordErrorMsgText = Resources.UserMsg.PasswordMismatch;
                return;
            }
            else
            {
                PasswordErrorMsgText = string.Empty;
                ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightGray];
            }
            _userModel.Password = UserPassword;
            await _autherization.CreateAccount(_userModel);

            if (_authentication.IsExist(_userModel))
            {
                _authentication.LastEmail = _userModel.Email;
                await NavigationService.NavigateAsync($"/{nameof(LogInPageView)}");
            }
        }
        #endregion

        public CreateAccountPassPageViewModel(
            INavigationService navigationService,
            IAutherization autherization,
            IAuthentication authentication)
            : base(navigationService)
        {
            _autherization = autherization;
            _authentication = authentication;

            TextResources = new TextResources(typeof(Resources.TextControls));

            ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightGray];

            backCommand = new DelegateCommand(BackCommandRelease);
            createAccountCommand = new DelegateCommand(CreateAccountCommandRelease);
        }

        #region -- Properties -- 

        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }

        private string _passwordErrorMsgText;
        public string PasswordErrorMsgText
        {
            get => _passwordErrorMsgText;
            set => SetProperty(ref _passwordErrorMsgText, value);
        }

        private Color _errorColor;
        public Color ErrorColor
        {
            get => _errorColor;
            set => SetProperty(ref _errorColor, value);
        }

        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        private string _userPasswordRepeat;
        public string UserPasswordRepeat
        {
            get => _userPasswordRepeat;
            set => SetProperty(ref _userPasswordRepeat, value);
        }
        #endregion

        #region -- Commands --
        private ICommand backCommand;
        public ICommand BackCommand { get => backCommand ?? new DelegateCommand(BackCommandRelease); }

        private ICommand createAccountCommand;
        public ICommand CreateAccountCommand { get=>createAccountCommand ?? new DelegateCommand(CreateAccountCommandRelease); }
        #endregion

        #region -- Overrides
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!parameters.ContainsKey(nameof(_userModel)))
            {
                throw new ArgumentNullException(nameof(_userModel));
            }
            _userModel = parameters.GetValue<UserModel>(nameof(_userModel));
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

    }
}
