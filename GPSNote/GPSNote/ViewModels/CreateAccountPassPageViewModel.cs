using GPSNote.Helpers;
using GPSNote.Models;
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
                                              IAutherization autherization)
            : base(navigationService)
        {
            BackCommand = new Command(BackCommandRelease);
            CreateAccountCommand = new Command(CreateAccountCommandRelease);

            _Autherization = autherization;

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
        private void CreateAccountCommandRelease()
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
            _Autherization.CreateAccount(_userModel);

            NavigationService.GoBackToRootAsync();
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
        #endregion
    }
}
