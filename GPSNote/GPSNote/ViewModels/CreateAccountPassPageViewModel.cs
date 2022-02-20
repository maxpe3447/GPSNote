using Acr.UserDialogs;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Authentication;
using GPSNote.Services.Autherization;
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
        public CreateAccountPassPageViewModel(INavigationService navigationService,
                                              IAutherization autherization,
                                              IAuthentication authentication)
            : base(navigationService)
        {
            BackCommand = new Command(BackCommandRelease);
            CreateAccountCommand = new Command(CreateAccountCommandRelease);

            _Autherization = autherization;
            _Authentication = authentication;

            TextResources = new TextResources(typeof(Resources.TextControls));

            ErrorColor = Color.Gray;
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
        public ICommand BackCommand { get; }
        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        public ICommand CreateAccountCommand { get; }
        private async void CreateAccountCommandRelease()
        {
            if(UserPassword != UserPasswordRepeat)
            {
                ErrorColor = Color.FromHex("#F24545");
                PasswordErrorMsgText = Resources.UserMsg.PasswordMismatch;
                return;
            }
            else
            {
                PasswordErrorMsgText = string.Empty;
                ErrorColor = Color.Gray;
            }
            _userModel.Password = UserPassword;
            await _Autherization.CreateAccount(_userModel);
            
            if(_Authentication.IsExistAsync(_userModel, out int id))
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(nameof(PinModel.UserId), id);
                await NavigationService.NavigateAsync($"/{nameof(Views.MainPage)}?createTab={nameof(Views.MapView)}&createTab={nameof(Views.PinListView)}", parameters);
            }
        }
        #endregion

        #region -- Overrides
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (!parameters.ContainsKey(nameof(_userModel)))
            {
                throw new ArgumentNullException(nameof(_userModel));
            }
            _userModel = parameters.GetValue<UserModel>(nameof(_userModel));
        }
        #endregion

        #region -- Private --
        private UserModel _userModel;
        private IAutherization _Autherization { get; }
        private IAuthentication _Authentication { get; }
        #endregion
    }
}
