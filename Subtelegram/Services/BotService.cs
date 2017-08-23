using System.Collections.Generic;
using Telegram.Bot;

namespace Subtelegram.Services
{
    public class BotService : IBotService
    {
        readonly BotConfiguration _config;

        public BotService(BotConfiguration config)
        {
            _config = config;
            Client = new TelegramBotClient(_config.BotToken);
        }

        public TelegramBotClient Client { get; }
    }
}