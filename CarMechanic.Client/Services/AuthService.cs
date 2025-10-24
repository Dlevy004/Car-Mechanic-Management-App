using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using CarMechanic.Shared.Models;
using CarMechanic.Client.Auth;

namespace CarMechanic.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            // login kérés az API-nak
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            // kiolvassuk a választ
            var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

            if (!response.IsSuccessStatusCode || loginResult == null || string.IsNullOrEmpty(loginResult.Token))
            {
                return new LoginResult { Token = null };
            }

            // sikeres login: a token save a Local Storage-be
            await _localStorage.SetItemAsStringAsync("authToken", loginResult.Token);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Token);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
