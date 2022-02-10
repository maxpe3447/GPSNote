using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Resources;
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
    class LogInPageViewModel : ViewModelBase
    {
        public LogInPageViewModel(INavigationService navigationService,
                               IRepository repository) 
            : base(navigationService)
        {
            Repository = repository;

            SigninCommand = new Command(SignInRelease);
            BackCommand = new  Command(BackCommandRelease);

            TextResources = new TextResources(typeof(Resources.TextControls));

            ErrorColor = Color.Gray;
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

        private Color _errorColor;
        public Color ErrorColor
        {
            get => _errorColor;
            set => SetProperty(ref _errorColor, value);
        }

        private string _emailErrorMsgText;
        public string EmailErrorMsgText
        {
            get => _emailErrorMsgText;
            set => SetProperty(ref _emailErrorMsgText, value);
        }

        private string _passwordErrorMsgText;
        public string PasswordErrorMsgText
        {
            get => _passwordErrorMsgText;
            set => SetProperty(ref _passwordErrorMsgText, value);
        }

        private TextResources _textResources;
        public TextResources TextResources
        {
            get => _textResources;
            set => SetProperty(ref _textResources, value);
        }
        #endregion

        #region -- Command --
        public ICommand SigninCommand { get; }
        private async void SignInRelease()
        {
            
            userModel = new UserModel
            {
                Email = UserEmail,
                Password = UserPassword
            };

            if (!string.IsNullOrEmpty(UserPassword) && !string.IsNullOrEmpty(UserEmail) && Repository.IsExistAsync(userModel, out int id))
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(nameof(PinModel.UserId), id);
                await NavigationService.NavigateAsync($"/{nameof(Views.MainPage)}?createTab={nameof(Views.MapView)}&createTab={nameof(Views.PinListView)}", parameters);
            }
            else
            {
                ErrorColor = Color.FromHex("#F24545");
                EmailErrorMsgText = UserMsg.WrongEmail;
                PasswordErrorMsgText = UserMsg.IncorrectPas;
            }
        }

        public ICommand BackCommand { get; }
        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }
        #endregion

        #region -- Override --
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(UserModel)))
            {
                var userModel = parameters.GetValue<UserModel>(nameof(UserModel));

                UserEmail = userModel.Email;
                UserPassword = userModel.Password;

            }
        }
        #endregion

        #region -- Private --
        UserModel userModel = null;
        IRepository Repository { get; }
        #endregion
    }
}
