using GPSNote.Helpers;
using GPSNote.Models;
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
            SignUpCommand = new Command(SignUpRelease);
            BackCommand = new  Command(BackCommandRelease);

            TextResources = new TextResources(typeof(Resources.TextControls));

            EntryBoardColor = Color.Gray;
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

        private Color _entryBoardColor;
        public Color EntryBoardColor
        {
            get => _entryBoardColor;
            set => SetProperty(ref _entryBoardColor, value);
        }

        private string _emailErrorMsgText;
        public string EmailErrorMsgText
        {
            get => _emailErrorMsgText;
            set => SetProperty(ref _emailErrorMsgText, value);
        }

        private Color _emailErrorMsgColor;
        public Color EmailErrorMsgColor
        {
            get => _emailErrorMsgColor;
            set => SetProperty(ref _emailErrorMsgColor, value);
        }

        private string _passwordErrorMsgText;
        public string PasswordErrorMsgText
        {
            get => _passwordErrorMsgText;
            set => SetProperty(ref _passwordErrorMsgText, value);
        }

        private Color _passwordErrorMsgColor;
        public Color PasswordErrorMsgColor
        {
            get => _passwordErrorMsgColor;
            set => SetProperty(ref _passwordErrorMsgColor, value);
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
                PasswordErrorMsgColor =  EmailErrorMsgColor =  EntryBoardColor = Color.FromHex("#F24545");
                EmailErrorMsgText = "Wrong Email";
                PasswordErrorMsgText = "The password is incorrect";
            }
        }

        public ICommand SignUpCommand { get; }
        private async void SignUpRelease()
        {
            await NavigationService.NavigateAsync("SignUpView");
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
