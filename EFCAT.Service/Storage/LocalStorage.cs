
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Text.Json;

namespace EFCAT.Service.Storage;

public interface ILocalStorage {
    Task<T> GetAsync<T>(string key);
    T Get<T>(string key);
    Task SetAsync<T>(string key, T value);
    void Set<T>(string key, T value);
    Task RemoveAsync(string key);
    void Remove(string key);
}

public class LocalStorage : ILocalStorage {
    private readonly IJSRuntime _jsRuntime;

    public LocalStorage(IJSRuntime jsRuntime) {
        _jsRuntime = jsRuntime;
    }

    // Get
    public async Task<T> GetAsync<T>(string key) => JsonSerializer.Deserialize<T>(await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key));
    public T Get<T>(string key) => Task.Run(() => GetAsync<T>(key)).Result;

    // Set
    public async Task SetAsync<T>(string key, T value) => await _jsRuntime.InvokeVoidAsync("localStorage.setItem", new object[] { key, JsonSerializer.Serialize(value) });
    public void Set<T>(string key, T value) => Task.Run(() => SetAsync(key, value));

    // Remove
    public async Task RemoveAsync(string key) => await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    public void Remove(string key) => Task.Run(() => RemoveAsync(key));
}

public static class LocalStorageExtensions {
    public static IServiceCollection AddLocalStorage(this IServiceCollection services) => services.AddScoped<ILocalStorage, LocalStorage>();
}