using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Text.Json;

namespace EFCAT.Service.Storage;

public interface ISessionStorage : IWebStorage { }

public class SessionStorage : ISessionStorage {
    private readonly IJSRuntime _jsRuntime;

    public SessionStorage(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    // Get
    public async Task<T> GetAsync<T>(string key) => JsonSerializer.Deserialize<T>(await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key));
    public T Get<T>(string key) => Task.Run(() => GetAsync<T>(key)).Result;

    // Get Keys
    public async Task<string[]> GetKeysAsync() => await _jsRuntime.InvokeAsync<string[]>("Object.keys(sessionStorage)");
    public string[] GetKeys() => Task.Run(() => GetKeysAsync()).Result;

    // Set
    public async Task SetAsync<T>(string key, T value) => await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", new object[] { key, JsonSerializer.Serialize(value) });
    public void Set<T>(string key, T value) => Task.Run(() => SetAsync(key, value));

    // Remove
    public async Task RemoveAsync(string key) => await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", key);
    public void Remove(string key) => Task.Run(() => RemoveAsync(key));

    // Clear
    public async Task ClearAsync() => await _jsRuntime.InvokeVoidAsync("sessionStorage.clear");
    public void Clear() => Task.Run(() => ClearAsync());

    // Length
    public async Task<int> LengthAsync() => await _jsRuntime.InvokeAsync<int>("Object.keys(sessionStorage).length");
    public int Length() => Task.Run(() => LengthAsync()).Result;
}

public static class SessionStorageExtensions {
    public static IServiceCollection AddSessionStorage(this IServiceCollection services) => services.AddScoped<ISessionStorage, SessionStorage>();
}