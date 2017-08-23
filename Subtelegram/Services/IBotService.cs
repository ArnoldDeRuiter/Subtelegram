using System.Collections.Generic;
using Telegram.Bot;

namespace Subtelegram.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}