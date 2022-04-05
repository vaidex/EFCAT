using EFCAT.Service.Authentication;
using Sample.Model.Entity;
using System.Net.Http.Headers;
using Sample.Model.Configuration;
using EFCAT.Service.Storage;

namespace Sample.UI.Services {
    public class MyAuthenticationService : AuthenticationService<User> {
        HttpClient _httpClient;

        public MyAuthenticationService(TestDbContext context, ILocalStorage localStorage, HttpClient client) : base(context, localStorage) {
            _httpClient = client;
        }

        protected override async Task OnAuthenticationSuccess(string token, User account) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        //protected override Task<Package> OnLogin(object obj, User account, string token) => Task.FromResult(new Package() { State = EState.ERROR, ErrorMessage = "This Error is displayed!" });
        //protected override Task<Package> OnRegister(User account) => Task.FromResult(new Package() { State = EState.ERROR, ErrorMessage = "Disallowed!" });
    }
}
