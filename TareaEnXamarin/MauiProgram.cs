namespace TodoApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Services
        builder.Services.AddSingleton<Services.DatabaseService>();

        // Register ViewModels
        builder.Services.AddTransient<ViewModels.MainPageViewModel>();
        builder.Services.AddTransient<ViewModels.AddEditTodoViewModel>();

        // Register Views
        builder.Services.AddTransient<Views.MainPage>();
        builder.Services.AddTransient<Views.AddEditTodoPage>();

        return builder.Build();
    }
}