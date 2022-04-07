using EFCAT.Service.Authentication;
using Sample.Model.Entity;
using System.Net.Http.Headers;
using Sample.Model.Configuration;
using EFCAT.Service.Storage;
using Microsoft.EntityFrameworkCore;

namespace Sample.UI.Services {
    public class MyAuthenticationService : AuthenticationService<User> {
        HttpClient _httpClient;

        public MyAuthenticationService(TestDbContext context, ILocalStorage localStorage, HttpClient client) : base(context, localStorage) {
            _httpClient = client;
            _roles.Add((user) => _dbSet.Where(o => o.Id == user.Id).Include(o => o.Roles).SelectMany(o => o.Roles.Select(o => o.RoleName)).ToListAsync());
            _roles.Add((user) => _dbSet.Where(o => o.Id == user.Id).Select(o => o.Id == 1 ? "Admin" : "").ToListAsync());
        }

        protected override async Task OnAuthenticationSuccess(string token, User account) {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }

        //protected override Task<Package> OnLogin(object obj, User account, string token) => Task.FromResult(new Package() { State = EState.ERROR, ErrorMessage = "This Error is displayed!" });
        //protected override Task<Package> OnRegister(User account) => Task.FromResult(new Package() { State = EState.ERROR, ErrorMessage = "Disallowed!" });
    }
}
