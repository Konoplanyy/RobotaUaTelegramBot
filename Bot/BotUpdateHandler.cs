using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RobotaUaTelegramBot.Bot
{
    class BotUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;

        public BotUpdateHandler(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            if (update.Type != UpdateType.Message || update.Message.Type != MessageType.Text)
                return;

            var message = update.Message;
            var chatId = message.Chat.Id;
            var text = message.Text.Trim();

            switch (text)
            {
                case "/start":
                    await _botClient.SendMessage(chatId, "Привіт! Я твій бот.");
                    break;

                case "/help":
                    await _botClient.SendMessage(chatId, "Доступні команди:\n/start - Почати\n/help - Допомога");
                    break;

                default:
                    await _botClient.SendMessage(chatId, "Невідома команда. Спробуй /help.");
                    break;
            }
        }
    }
}
