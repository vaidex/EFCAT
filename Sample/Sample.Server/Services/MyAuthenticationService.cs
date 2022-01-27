using EFCAT.Service.Authentication;
using Sample.Model.Entity;
using System.Net.Http.Headers;
using Sample.Model.Configuration;
using EFCAT.Service.Storage;

namespace Sample.Server.Services {
    public class MyAuthenticationService : AuthenticationService<User> {
        HttpClient _httpClient;

        public MyAuthenticationService(TestDbContext context, ILocalStorage localStorage, HttpClient client) : base(context, localStorage) {
            _httpClient = client;
        }

        protected override async Task OnAuthenticationSuccess(string token) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
    }
}
