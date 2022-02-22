using Acr.UserDialogs;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Resources;
using GPSNote.Services.Authentication;
using GPSNote.Services.Repository;
using GPSNote.Services.Settings;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    class LogInPageViewModel : ViewModelBase
    {
        public LogInPageViewModel(INavigationService navigationService,
                                  IAuthentication authentication,
                                  ISettingsManager settingManager) 
            : base(navigationService)
        {
            _Authentication = authentication;
            _settingsManager = settingManager;

            SigninCommand = new Command(SignInRelease);
            BackCommand = new  Command(BackCommandRelease);
            GoogleAuthCommand = new Command(GoogleAuthCommandRelease);

            TextResources = new TextResources(typeof(TextControls));

            ErrorColor = (Color)App.Current.Resources[ColorsName.LightGray];
        }

        #region -- Properties -- 
        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail ?? _settingsManager.LastEmail;
            set => SetProperty(ref _userEmail, value);
        }

        private string _userPassword;
        public string UserPassword
        {
            get => _userPassword ?? _settingsManager.LastPassword;
            set => SetProperty(ref _userPassword, value);
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
            
            _userModel = new UserModel
            {
                Email = UserEmail,
                Password = UserPassword
            };

            if (!string.IsNullOrEmpty(UserPassword) && !string.IsNullOrEmpty(UserEmail) && _Authentication.IsExistAsync(_userModel, out int id))
            {
                _settingsManager.LastEmail = UserEmail;
                _settingsManager.LastPassword = UserPassword;

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(nameof(PinModel.UserId), id);
                
                await NavigationService.NavigateAsync($"/{nameof(Views.MainPage)}?createTab={nameof(Views.MapView)}&createTab={nameof(Views.PinListView)}", parameters);
                
            }
            else
            {
                ErrorColor = (Color)App.Current.Resources[Resources.ColorsName.LightRed];
                EmailErrorMsgText = UserMsg.WrongEmail;
                PasswordErrorMsgText = UserMsg.IncorrectPas;
            }
        }

        public ICommand BackCommand { get; }
        private async void BackCommandRelease()
        {
            await NavigationService.GoBackAsync();
        }

        public ICommand GoogleAuthCommand { get; }
        private void GoogleAuthCommandRelease()
        {
            //GoogleAuth.GoogleAuthentication();
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
        private UserModel _userModel = null;
        private IAuthentication _Authentication { get; }
        private ISettingsManager _settingsManager { get; }
        private LinkModel _LinkModel { get; set; } = null;
        #endregion
    }
}
