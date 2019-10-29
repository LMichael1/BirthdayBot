using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class AllCommand : ICommand
    {
        public string Name => @"/all";

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            AppDbContext context = new AppDbContext();

            var result = await context.GetUsers(String.Empty, chatId);

            if (result.Count() > 0)
            {
                StringBuilder sb = new StringBuilder("*Все даты рождения:*\n");
                foreach (var i in result)
                {
                    sb.AppendFormat("{0}: *{1}.{2}*\n", i.Name, i.Birthday.Day, i.Birthday.Month);
                }

                await botClient.SendTextMessageAsync(chatId, sb.ToString(),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Список пуст.",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
