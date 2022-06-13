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
        readonly private IAutherizationService _autherization;
        readonly private IAuthenticationService _authentication;

        private UserModel _userModel;
        private LinkModel _LinkModel { get; set; } = null;

        public CreateAccountPassPageViewModel(
            INavigationService navigationService,
            IAutherizationService autherization,
            IAuthenticationService authentication)
            : base(navigationService)
        {
            _autherization = autherization;
            _authentication = authentication;

            TextControlsResources = new TextResources(typeof(Resources.TextControls));
            TextUserMsgResources = new TextResources(typeof(Resources.UserMsg));

            IsPasswordValid = true;
            //ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightGray];
        }

        #region -- Properties -- 

        private TextResources _textControlsResources;
        public TextResources TextControlsResources
        {
            get => _textControlsResources;
            set => SetProperty(ref _textControlsResources, value);
        }

        private TextResources _textUserMsgResources;
        public TextResources TextUserMsgResources
        {
            get => _textUserMsgResources;
            set => SetProperty(ref _textUserMsgResources, value);
        }

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => SetProperty(ref _isPasswordValid, value);
        }

        //private string _passwordErrorMsgText;
        //public string PasswordErrorMsgText
        //{
        //    get => _passwordErrorMsgText;
        //    set => SetProperty(ref _passwordErrorMsgText, value);
        //}

        //private Color _errorColor;
        //public Color ErrorColor
        //{
        //    get => _errorColor;
        //    set => SetProperty(ref _errorColor, value);
        //}

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

        private ICommand backCommand;
        public ICommand BackCommand { get => backCommand ??= new DelegateCommand(BackCommandRelease); }

        private ICommand createAccountCommand;
        public ICommand CreateAccountCommand { get=>createAccountCommand ??= new DelegateCommand(CreateAccountCommandRelease); }
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

        #region Private
        private async void BackCommandRelease()
        {
            await NavigationService.NavigateAsync($"/{nameof(CreateAnAccountView)}");
        }
        private async void CreateAccountCommandRelease()
        {
            //if (UserPassword != UserPasswordRepeat)
            //{
            //    ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightRed];
            //    PasswordErrorMsgText = Resources.UserMsg.PasswordMismatch;
            //    return;
            //}
            //else
            //{
            //    PasswordErrorMsgText = string.Empty;
            //    ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightGray];
            //}
            IsPasswordValid = !(UserPassword != UserPasswordRepeat);
            if (!IsPasswordValid) return;

            _userModel.Password = UserPassword;
            await _autherization?.CreateAccount(_userModel);

            if (_authentication.IsExist(_userModel))
            {
                _authentication.LastEmail = _userModel.Email;
                await NavigationService.NavigateAsync($"/{nameof(LogInPageView)}");
            }
        }
        #endregion
    }
}
