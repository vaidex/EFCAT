﻿using EFCAT.Model.Data.Converter;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace EFCAT.Model.Data;

[NotMapped]
[JsonConverter(typeof(CryptJsonFactory))]
[TypeDescriptionProvider(typeof(CryptTypeDescriptionProvider))]
public class Crypt<TAlgorithm> : ValueObject, IEquatable<object> where TAlgorithm : IAlgorithm, new() {
    private bool IsCrypted { get; set; } = false;
    private string value;
    private string Value { get => this.value; set { this.value = value; IsCrypted = false; } }

    TAlgorithm _algorithm = new TAlgorithm();

    public Crypt() { }
    public Crypt(string value, bool isCrypted) { Value = value; IsCrypted = isCrypted; }

    public static implicit operator Crypt<TAlgorithm>(string value) => String.IsNullOrEmpty(value) ? new Crypt<TAlgorithm>() : new Crypt<TAlgorithm>(value, false);
    public static explicit operator string(Crypt<TAlgorithm> value) => value.ToString();

    public string Encrypt() {
        if (IsCrypted) return Value;
        Value = Encrypt(Value);
        IsCrypted = true;
        return Value;
    }
    private string Encrypt(string value) => _algorithm.Encrypt(value);
    public string Decrypt() => IsCrypted ? _algorithm.Decrypt(Value) : Value;

    public bool Verify(string value) => IsCrypted ? Encrypt(value) == Value : value == Value;

    public override string ToString() => Value;
    public override bool Equals(object? other) => other is string str ? Verify(str) : false;
    public override int GetHashCode() => (Value).GetHashCode();
}

public interface IAlgorithm {
    int? Size { get; }
    string Encrypt(string value);
    string Decrypt(string value);
}

// Hash Algorithms
public abstract class HashAlgorithm : IAlgorithm {
    private System.Security.Cryptography.HashAlgorithm _algorithm;
    public abstract int? Size { get; }

    public HashAlgorithm(System.Security.Cryptography.HashAlgorithm algorithm) { _algorithm = algorithm; }

    public string Encrypt(string value) {
        if (String.IsNullOrEmpty(value)) return "";
        StringBuilder sb = new StringBuilder();

        Byte[] result = _algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
        foreach(Byte b in result) sb.Append(b.ToString("x2"));

        return sb.ToString();
    }

    public string Decrypt(string value) => value;
}
public sealed class SHA256 : HashAlgorithm {
    public SHA256() : base(System.Security.Cryptography.SHA256.Create()) { }
    public override int? Size => 256;
}
public sealed class SHA512 : HashAlgorithm {
    public SHA512() : base(System.Security.Cryptography.SHA512.Create()) { }
    public override int? Size => 512;
}

public class AesAlgorithm : IAlgorithm {
    public int? Size => null;

    readonly string _key = "";

    public AesAlgorithm(string key) {
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
