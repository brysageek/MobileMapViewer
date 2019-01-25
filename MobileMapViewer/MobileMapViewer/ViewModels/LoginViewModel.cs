using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MobileMapViewer.Services;
using System;
using System.Windows.Input;

namespace MobileMapViewer.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly INavigationService _navigationService;
        private readonly string _pageKey;
        private bool _isAuthenticated;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                RaisePropertyChanged(() => IsAuthenticated);
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private string _status = "Please Sign In";
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        private readonly RelayCommand<string> _loginCommand;
        public ICommand LoginCommand => _loginCommand;

        public LoginViewModel(IAuthService authService, INavigationService navigationService, string pageKey)
        {
            _authService = authService;
            _navigationService = navigationService;
            _pageKey = pageKey;

            _loginCommand = new RelayCommand<string>(async p =>
            {
                if (p != null)
                {
                    _password = p;
                }
                try
                {
                    IsBusy = true;
                    Status = "Signing In...";

                    IsAuthenticated = await _authService.Authenticate(_userName, _password);

                    IsBusy = false;
                    Status = "User Logged in";

                    _navigationService.NavigateTo(_pageKey, null);
                }
                catch (Exception)
                {
                    IsAuthenticated = false;
                    Status = "Unable to Sign In, Please check username and password";
                    IsBusy = false;
                }
            });
        }
    }
}

