using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class WeekCommand : ICommand
    {
        public string Name => @"/week";

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
            var items = result.Where(i => i.Birthday.Date.AddYears(-i.Birthday.Year + 1) >= DateTime.Today.AddYears(-DateTime.Today.Year + 1) &&
                                i.Birthday.Date.AddYears(-i.Birthday.Year + 1) <= DateTime.Today.AddYears(-DateTime.Today.Year + 1).AddDays(7))
                                    .OrderBy(i => i.Birthday);

            if (items.Count() > 0)
            {
                StringBuilder sb = new StringBuilder("*Дни рождения в ближайшую неделю:*\n");
                foreach (var i in items)
                {
                    sb.AppendFormat("{0}: *{1}.{2}*\n", i.Name, i.Birthday.Day, i.Birthday.Month);
                }

                await botClient.SendTextMessageAsync(chatId, sb.ToString(),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Именинники не найдены.",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
