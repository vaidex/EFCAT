using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EFCAT.Service.Authentication;

public class AuthenticationAttribute : ValidationAttribute {
    private EAuthenticationType authenticationType;
    public AuthenticationAttribute(string type) : this(Enum.Parse<EAuthenticationType>(type)) { }
    public AuthenticationAttribute(EAuthenticationType type) {
        authenticationType = type;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext context) {
        switch (authenticationType) {
            case EAuthenticationType.LOGIN:
                return (bool)Get("Login", new object[] { value }) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            case EAuthenticationType.REGISTER:
                return (bool)Get("Register", new object[] { value }) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            default:
                Get("Logout");
                return ValidationResult.Success;
        }
    }
    private object? Get(string methodName, object[]? parameters = null) {
        AuthenticationStateProvider? provider = AuthenticationSettings.provider;
        if (provider == null) return false;
        MethodInfo? method = provider.GetType().GetMethod(methodName);
        return method.Invoke(provider, parameters);
    }
}

public enum EAuthenticationType {
    LOGIN, REGISTER, LOGOUT
}

public static class AuthenticationSettings {
    public static AuthenticationStateProvider? provider;
}