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
        readonly private IAuthenticationService _authentication;
        private LinkModel _LinkModel { get; set; } = null;

        public LogInPageViewModel(
            INavigationService navigationService,
            IAuthenticationService authentication) 
            : base(navigationService)
        {
            _authentication = authentication;

            TextControlsResources = new TextResources(typeof(TextControls));
            TextUserMsgResources = new TextResources(typeof(UserMsg));

            IsPasswordValid = IsEmailValid = true;
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

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set => SetProperty(ref _isEmailValid, value);
        }

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => SetProperty(ref _isPasswordValid, value);
        }

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

        public ICommand signinCommand;
        public ICommand SigninCommand { get => signinCommand ??= new DelegateCommand(SignInRelease); }

        public ICommand backCommand;
        public ICommand BackCommand { get => backCommand ??= new DelegateCommand(BackCommandRelease); }


        public ICommand googleAuthCommand;
        public ICommand GoogleAuthCommand { get => googleAuthCommand ??= new DelegateCommand(GoogleAuthCommandRelease); }
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

            IsEmailValid = !string.IsNullOrEmpty(UserEmail) && _authentication.IsExistEmail(userModel.Email);
            IsPasswordValid = !string.IsNullOrEmpty(UserPassword) && _authentication.IsExist(userModel);
            
            if(!(IsEmailValid && IsPasswordValid))
            {
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
