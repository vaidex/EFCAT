using Microsoft.AspNetCore.Components.WebView.Maui;
using Sample.MAUI.Data;
using Sample.UI;

namespace Sample.MAUI;
public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .RegisterBlazorMauiWebView()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddBlazorWebView();
        builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddUI();

        return builder.Build();
    }
}
