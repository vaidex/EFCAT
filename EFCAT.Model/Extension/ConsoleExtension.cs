
namespace EFCAT.Model.Extension;

internal static class ConsoleExtension {
    internal static void PrintInfo(this bool write, string s) {
        if (!write) return;
        s = $"[EFCAT] {s}";
        System.Diagnostics.Debug.WriteLine(s);
        System.Console.WriteLine(s);
    }
}
