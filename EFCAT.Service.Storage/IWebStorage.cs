
namespace EFCAT.Service.Storage;

public interface IWebStorage {
    Task<T> GetAsync<T>(string key);
    T Get<T>(string key);

    Task<string[]> GetKeysAsync();
    string[] GetKeys();

    Task SetAsync<T>(string key, T value);
    void Set<T>(string key, T value);

    Task RemoveAsync(string key);
    void Remove(string key);

    Task ClearAsync();
    void Clear();

    Task<int> LengthAsync();
    int Length();
}