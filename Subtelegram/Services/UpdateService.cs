using System;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Subtelegram.Services
{
    public class UpdateService : IUpdateService
    {
        readonly IBotService _botService;

        public UpdateService(IBotService botService)
        {
            _botService = botService;
        }

        public void HandleUpdate(Update update)
        {
			Console.WriteLine($"Received Message from {update.Message.Chat.Id}");

            if (update.Message.Type == MessageType.TextMessage) {
                HandleTextMessageUpdate(update);
            } else if (update.Message.Type == MessageType.PhotoMessage 
                       || update.Message.Type == MessageType.VideoMessage 
                       || update.Message.Type == MessageType.DocumentMessage) {
                HandleMediaMessageUpdate(update);
            }
        }

        void HandleTextMessageUpdate(Update update) 
        {
			if (update.Message.Text.StartsWith("/r/", StringComparison.OrdinalIgnoreCase)) {
				ReplyWithSubredditURL(update);
			} else if (update.Message.Text.StartsWith("/help", StringComparison.OrdinalIgnoreCase)) {
				ReplyWithHelpMessage(update);
			} else {
				Console.WriteLine($"\tNot a message for me");
			}
        }

        void HandleMediaMessageUpdate(Update update)
        {
            if (update.Message.Caption == null) {
                Console.WriteLine($"\tNo caption for media message");
                return;
            }

            if (update.Message.Caption.StartsWith("/r/", StringComparison.OrdinalIgnoreCase)) {
                ReplyWithSubredditURL(update);
            } else {
                Console.WriteLine($"\tNot a media message for me");
            }
        }


        void ReplyWithHelpMessage(Update update)
        {
			var message = update.Message;
            Console.WriteLine($"\tSending help message to: {message.Chat.Id}");
            _botService.Client.SendTextMessageAsync(
                message.Chat.Id, "Just send me a message starting with `/r/SubredditName` and I will send you the URL back!", ParseMode.Markdown).GetAwaiter();
        }

        void ReplyWithSubredditURL(Update update)
        {
            var message = update.Message;

            var subreddit = message.Text ?? message.Caption;

            // remove /r/ from string
            subreddit = subreddit.Substring(3);

            // remove possible text after subreddit name
            if (subreddit.Any(Char.IsWhiteSpace)) {
                Console.WriteLine($"\tWhitespace detected, removing excess...");
                subreddit = subreddit.Substring(0, subreddit.IndexOf(" ", StringComparison.OrdinalIgnoreCase));
			}

			if (!Regex.IsMatch(subreddit, "^[a-zA-Z0-9_]{1,20}$")) {
				Console.WriteLine($"\tNot a valid Subreddit name: {subreddit}");
				return;
			}

			Console.WriteLine($"\tValid Subreddit name: {subreddit}");

			Console.WriteLine($"\tSending message to: {message.Chat.Id}");
			_botService.Client.SendTextMessageAsync(message.Chat.Id, $"https://reddit.com/r/{subreddit}").GetAwaiter();
        }
    }
}
