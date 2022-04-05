
namespace EFCAT.Service.Authentication;

public class Package {
    public EState State { get; set; } = EState.OK;
    public bool Success => State == EState.OK;
    public string? ErrorMessage { get; set; }

    public static explicit operator bool(Package value) => value.Success;
}

internal static class PackageExtensions {
    internal static Package Copy(this Package From, Package To) {
        To.State = From.State;
        To.ErrorMessage = From.ErrorMessage;
        return To;
    }
}

public enum EState {
    OK, ERROR
}