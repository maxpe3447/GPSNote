using Acr.UserDialogs;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Resources;
using GPSNote.Services.Authentication;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using GPSNote.Views;
using Prism.Commands;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    class LogInPageViewModel : ViewModelBase
    {
        readonly private IAuthentication _authentication;
        private LinkModel _LinkModel { get; set; } = null;

        public LogInPageViewModel(
            INavigationService navigationService,
            IAuthentication authentication) 
            : base(navigationService)
        {
            _authentication = authentication;

            TextResources = new TextResources(typeof(TextControls));

            ErrorColorEmail = ErrorColor = (Color)App.Current.Resources[ColorsName.LightGray];
            
            signinCommand = new DelegateCommand(SignInRelease);
            backCommand = new DelegateCommand(BackCommandRelease);
            googleAuthCommand = new DelegateCommand(GoogleAuthCommandRelease);
        }

        #region -- Properties -- 
        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail ?? _authentication.LastEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword;
            set => SetProperty(ref _userPassword, value);
        }

        private Color _errorColor;
        public Color ErrorColor
        {
            get => _errorColor;
            set => SetProperty(ref _errorColor, value);
        }

        private Color _errorColorEmail;
        public Color ErrorColorEmail
        {
            get => _errorColorEmail;
            set => SetProperty(ref _errorColorEmail, value);
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

        public ICommand signinCommand;
        public ICommand SigninCommand { get => signinCommand ?? new DelegateCommand(SignInRelease); }

        public ICommand backCommand;
        public ICommand BackCommand { get => backCommand ?? new DelegateCommand(BackCommandRelease); }


        public ICommand googleAuthCommand;
        public ICommand GoogleAuthCommand { get => googleAuthCommand ?? new DelegateCommand(GoogleAuthCommandRelease); }
        #endregion

        #region -- Override --
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
        private async void SignInRelease()
        {
            var userModel = new UserModel
            {
                Email = UserEmail,
                Password = UserPassword
            };

            PasswordErrorMsgText = EmailErrorMsgText = string.Empty;
            ErrorColorEmail = ErrorColor = (Color)App.Current.Resources[ColorsName.LightGray];

            if (!string.IsNullOrEmpty(UserEmail) && !_authentication.IsExistEmail(userModel.Email))
            {
                ErrorColorEmail = (Color)App.Current.Resources[ColorsName.LightRed];
                EmailErrorMsgText = UserMsg.WrongEmail;
                return;
            }
            if (!string.IsNullOrEmpty(UserPassword) && !_authentication.IsExist(userModel))
            {
                ErrorColor = (Color)App.Current.Resources[ColorsName.LightRed];
                PasswordErrorMsgText = UserMsg.IncorrectPas;
                return;
            }

            _authentication.LastEmail = UserEmail;
            await NavigationService.NavigateAsync($"/{nameof(MainPage)}?createTab={nameof(MapView)}&createTab={nameof(PinListView)}");
        }
        private async void BackCommandRelease()
            => await NavigationService.NavigateAsync($"/{nameof(StartPageView)}");
        private void GoogleAuthCommandRelease()
        {
            //GoogleAuth.GoogleAuthentication();
        }
        #endregion
    }
}
