using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class StartCommand : ICommand
    {
        public string Name => @"/start";

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            await botClient.SendTextMessageAsync(chatId, "Hallo I'm ASP.NET Core Bot", parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
