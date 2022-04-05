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
            case EAuthenticationType.REGISTER:
                string request = authenticationType == EAuthenticationType.LOGIN ? "Login" : "Register";
                Package? package = Get(request, new object[] { value }) as Package;
                if(package == null) return new ValidationResult(ErrorMessage);
                return package.Success ? ValidationResult.Success : new ValidationResult(package.ErrorMessage ?? ErrorMessage);
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