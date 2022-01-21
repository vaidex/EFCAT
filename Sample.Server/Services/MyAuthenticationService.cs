using EFCAT.Domain.Repository;
using EFCAT.Service.Authentication;
using Sample.Model.Entity;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using Sample.Model.Configuration;
using EFCAT.Service.Storage;

namespace Sample.Server.Services {
    public class MyAuthenticationService : AuthenticationService<User> {
        HttpClient _httpClient;
        ILocalStorage _localStorage;

        public MyAuthenticationService(TestDbContext context, ILocalStorage localStorage, HttpClient client) : base(context) {
            _localStorage = localStorage;
            _httpClient = client;
        }

        protected override async Task<string> ReadAsync(string item) => await _localStorage.GetAsync<string>(item);
        protected override async Task RemoveAsync(string item) => await _localStorage.RemoveAsync(item);
        protected override async Task WriteAsync(string item, string value) => await _localStorage.SetAsync(item, value);

        protected override async Task OnAuthenticationSuccess(string token) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
    }
}
