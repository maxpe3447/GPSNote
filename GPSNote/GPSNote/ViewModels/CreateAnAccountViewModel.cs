using System.Windows.Input;
using GPSNote.Helpers;
using GPSNote.Models;
using GPSNote.Services.Authentication;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace GPSNote.ViewModels
{
    public class CreateAnAccountViewModel : ViewModelBase
    {
        private UserModel _userModel;
        private LinkModel _LinkModel { get; set; } = null;
        private IAuthenticationService _authentication;

        public CreateAnAccountViewModel(INavigationService navigationService,
                                        IAuthenticationService authentication)
            :base(navigationService)
        {
            _authentication = authentication;

            IsEmailValid = true;

            TextControlsResources = new TextResources(typeof(Resources.TextControls));
            TextUserMsgResources = new TextResources(typeof(Resources.UserMsg));
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

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set => SetProperty(ref _isEmailValid, value);
        }

        private ICommand nextCommand;
        public ICommand NextCommand { get => nextCommand ??= new DelegateCommand(NextCommandRelease); }

        private ICommand backCommand;
        public ICommand BackCommand { get => backCommand ??= new DelegateCommand(BackCommandRelease); }

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
            IsEmailValid = !_authentication.IsExistEmail(UserEmail);
            if (!IsEmailValid) return;
            
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