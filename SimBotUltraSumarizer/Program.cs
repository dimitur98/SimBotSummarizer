using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using SimBotTelegram.Api.Ver1;
using SimBotUltraSummarizer.Helpers.Log;
using System.Globalization;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

logger.Debug("Starting app");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    SimBotUltraSummarizerDb.Dal.Db.SetupConnection(builder.Configuration.GetConnectionString("SimBotAnalyzerDb"));

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(30));
    builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);


    builder.Services.AddSingleton<ILogHelper, LogHelper>();
    builder.Services.AddSingleton<SimBotTelegramApi>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AllAllowed",
                      policy =>
                      {
                          policy.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                      });
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }


    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseCors("AllAllowed");


    app.UseAuthorization();

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Signals}/{action=Search}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
