using CG.Orange.Host.Hubs;
using Serilog;

//BootstrapLogger.LogLevelToDebug();

try
{

    // Log what we are about to do.
    BootstrapLogger.Instance().LogInformation(
        "Starting up {name}",
        AppDomain.CurrentDomain.FriendlyName
        );

    // Create an application builder.
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog stuff.
    builder.Host.UseSerilog((ctx, lc) =>
    {
        lc.ReadFrom.Configuration(ctx.Configuration);
    });

    // Add Blazor stuff.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddControllers().AddApplicationPart(
        typeof(CG.Orange.Controllers.SettingsController).Assembly
        );

    // Add MudBlazor stuff
    builder.Services.AddMudServices();

    // Add Orange stuff.
    builder.AddDataAccess(bootstrapLogger: BootstrapLogger.Instance())
        .AddRepositories(bootstrapLogger: BootstrapLogger.Instance())
        .AddManagers(bootstrapLogger: BootstrapLogger.Instance())
        .AddSeeding<SeedDirector>(bootstrapLogger: BootstrapLogger.Instance())
        .AddBlazorPlugins(bootstrapLogger: BootstrapLogger.Instance())
        .AddOrangeIdentity(bootstrapLogger: BootstrapLogger.Instance())
        .AddOrangeServices(bootstrapLogger: BootstrapLogger.Instance());

    // Build the application.
    var app = builder.Build();

    // Setup the proper environment.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    else
    {
        app.UseSerilogRequestLogging();
    }

    // Use Blazor stuff.
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapBlazorHub();
    app.MapHub<SignalRHub>("/_backchannel");
    app.MapFallbackToPage("/_Host");
    app.MapControllers();
    
    // Use Orange stuff.
    app.UseDataAccess()
        .UseOrangeIdentity()
        .UseSeeding()
        .UseBlazorPlugins();

    // Run the application.
    app.Run();
}
catch (Exception ex)
{
    // Log the error.
    BootstrapLogger.Instance().LogCritical(
        ex,
        "Unhandled exception: {msg}!",
        ex.GetBaseException().Message
        );
}
finally
{
    // Log what we are doing.
    BootstrapLogger.Instance().LogInformation(
        "Shutting down"
        );
}


