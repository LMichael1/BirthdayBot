using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class AddBirthdayCommand : ICommand
    {
        public string Name => @"/addbirthday";

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var str = message.Text.Split(new char[] { ',', ';' });

            if (str.Length == 3)
            {
                var m = "Успешно добавлено: " + str[1] + " " + str[2];
                await botClient.SendTextMessageAsync(chatId, m,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }

            await botClient.SendTextMessageAsync(chatId, "Hello, " + message.From.FirstName + "! I'm ASP.NET Core Bot",
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
