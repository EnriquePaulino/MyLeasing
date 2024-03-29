﻿using MyLeasing.Common.Services;
using MyLeasing.Common.ViewModels;
using Prism.Commands;
using Prism.Navigation;

namespace MyLeasing.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;

        public LoginPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;

            //TODO: delete this line
            Email = "jzuluaga55@hotmail.com";
            Password = "123456";
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public string Email { get; set; }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a email",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password",
                    "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);
            if (!connection)
            {
                IsEnabled = true;
                IsRunning = false;
                await App.Current.MainPage.DisplayAlert(
                    "Error", 
                    "Check the internet connection.", 
                    "Accept");
                return;
            }

            var request = new TokenRequest
            {
                Password = Password,
                Username = Email
            };

            var response = await _apiService.GetTokenAsync(
                url, 
                "Account", 
                "/CreateToken", request);

            if (!response.IsSuccess)
            {
                IsEnabled = false;
                IsRunning = true;
                await App.Current.MainPage.DisplayAlert(
                    "Error", 
                    "User or password incorrect.", 
                    "Accept");
                Password = string.Empty;
                return;
            }

            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(
                url,
                "api",
                "/Owners/GetOwnerByEmail",
                "bearer",
                token.Token,
                Email);

            if (!response2.IsSuccess)
            {
                IsEnabled = false;
                IsRunning = true;
                await App.Current.MainPage.DisplayAlert(
                    "Error", 
                    "Problem with user data, call 1-800-EAT-SHIT.", 
                    "Accept");
                Password = string.Empty;
                return;
            }

            var owner = response2.Result;
            var parameters = new NavigationParameters
            {
                {"owner", owner}
            };

            await _navigationService.NavigateAsync("PropertiesPage", parameters);
            IsEnabled = true;
            IsRunning = false;
        }
    }
}
