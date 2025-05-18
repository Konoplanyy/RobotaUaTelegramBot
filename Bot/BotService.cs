using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
namespace RobotaUaTelegramBot.Bot
{
    class BotService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly BotUpdateHandler _updateHandler;

        public BotService(ITelegramBotClient botClient, BotUpdateHandler updateHandler)
        {
            _botClient = botClient;
            _updateHandler = updateHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // отримувати всі типи оновлень
            };

            _botClient.StartReceiving(
                updateHandler: _updateHandler,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken
            );

            var me = await _botClient.GetMeAsync(stoppingToken);
            Console.WriteLine($"Бот {me.Username} запущено.");
        }
    }
}
