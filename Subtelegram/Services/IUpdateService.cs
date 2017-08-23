using Telegram.Bot.Types;

namespace Subtelegram.Services
{
    public interface IUpdateService
    {
        void HandleUpdate(Update update);
    }
}
