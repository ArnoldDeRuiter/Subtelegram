using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Subtelegram.Services
{
    public class UpdateService : IUpdateService
    {
        readonly IBotService _botService;

        public UpdateService(IBotService botService){
            _botService = botService;
        }

        public void HandleUpdate(Update update)
        {
			Console.WriteLine($"Received Message from {update.Message.Chat.Id}");

            if (update.Message.Type == MessageType.TextMessage) {
                if (update.Message.Text.StartsWith("/r/", StringComparison.OrdinalIgnoreCase)){
                    ReplyWithSubredditURL(update);
				}
                else {
                    Console.WriteLine($"\tNot a message for me");
                }
			}
        }

        void ReplyWithSubredditURL(Update update){
            var message = update.Message;
			var subreddit = message.Text.Substring(3);

			if (!Regex.IsMatch(subreddit, "^[a-zA-Z0-9_]{0,20}$") || string.IsNullOrEmpty(subreddit))
			{
				Console.WriteLine($"\tNot a valid Subreddit name: {subreddit}");
				return;
			}

			Console.WriteLine($"\tValid Subreddit name: {subreddit}");

			Console.WriteLine($"\tSending message to: {message.Chat.Id}");
			_botService.Client.SendTextMessageAsync(message.Chat.Id, $"https://reddit.com/r/{subreddit}").GetAwaiter();
        }
    }
}
