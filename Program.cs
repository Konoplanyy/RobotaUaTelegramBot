using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RobotaUaTelegramBot.Bot;
using RobotaUaTelegramBot.Data.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace RobotaUaTelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    // Прив’язка конфігурації
                    services.Configure<BotSettings>(context.Configuration.GetSection("BotSettings"));
                    services.Configure<APISettings>(context.Configuration.GetSection("APISettings"));

                    // TelegramBotClient з токеном з конфігурації
                    services.AddSingleton<ITelegramBotClient>(sp =>
                    {
                        var botSettings = sp.GetRequiredService<IOptions<BotSettings>>().Value;
                        return new TelegramBotClient(botSettings.TelegramApiKey);
                    });

                    // Реєстрація твого обробника команд, фонових сервісів, сховищ і т.д.
                    services.AddSingleton<BotUpdateHandler>();
                    //services.AddHostedService<BackgroundChecker>();

                    // Додаткові сервіси
                    // services.AddScoped<IUserStorage, EfUserStorage>();
                    // services.AddScoped<IApiClient, ApiClient>();
                })
                .Build();

            await host.RunAsync();
        }
    }
}
