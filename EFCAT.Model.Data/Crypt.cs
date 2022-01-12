using System.Security.Cryptography;
using System.Text;

namespace EFCAT.Model.Data;

public class Crypt<TAlgorithm> where TAlgorithm : IAlgorithm, new() {

    private string? _value;

    public string Value { get => Get(); set => Set(value); }

    TAlgorithm _algorithm = new TAlgorithm();

    public Crypt() { }

    public Crypt(string value) => _value = value;

    public static implicit operator Crypt<TAlgorithm>(string value) => value == null ? new Crypt<TAlgorithm>() : new Crypt<TAlgorithm>() { Value = value };

    private string Encrypt(string value) => _algorithm.Encrypt(value);

    public string Decrypt() => _algorithm.Decrypt(_value ?? "");

    public bool Verify(string value) => Encrypt(value) == Get();

    public string Get() => _value ?? "";
    public void Set(string value) => _value = Encrypt(value);

    public override string ToString() => Get();
}

public interface IAlgorithm {
    string Encrypt(string value);
    string Decrypt(string value);
}

public class Algorithm : IAlgorithm {
    private HashAlgorithm _algorithm;

    public Algorithm(HashAlgorithm algorithm) { _algorithm = algorithm; }

    public string Encrypt(string value) {
        StringBuilder sb = new StringBuilder();

        Byte[] result = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
        foreach(Byte b in result) sb.Append(b.ToString("x2"));

        return sb.ToString();
    }

    public string Decrypt(string value) {
        throw new NotImplementedException();
    }
}

public class SHA256 : Algorithm {
    public SHA256() : base(System.Security.Cryptography.SHA256.Create()) { }
}

public class SHA512 : Algorithm {
    public SHA512() : base(System.Security.Cryptography.SHA512.Create()) { }
}

public class CustomAlgorithm : IAlgorithm {

    readonly string _key = "";

    public CustomAlgorithm(string key) {
        _key = key;
    }
    
    public string Encrypt(string value) {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create()) {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream()) {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream)) {
                        streamWriter.Write(value);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public string Decrypt(string value) {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(value);

        using (Aes aes = Aes.Create()) {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer)) {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read)) {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream)) {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    
}