using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class GetUserCommand : ICommand
    {
        public string Name => @"/getuser";

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

            var nameArray = message.Text.Split(' ');
            string name = String.Empty;

            for (int i = 1; i < nameArray.Length; i++)
            {
                name += nameArray[i];

                if (i > 1 && i < nameArray.Length - 1)
                {
                    name += " ";
                }
            }

            if (name != String.Empty)
            {
                var result = await context.GetUsers(name, chatId);

                if (result.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder("_*Результат:*_\n");
                    foreach (var i in result)
                    {
                        sb.AppendFormat("{0}: *{1}.{2}*\n", i.Name, i.Birthday.Day, i.Birthday.Month);
                    }

                    await botClient.SendTextMessageAsync(chatId, sb.ToString(),
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Поиск не дал результатов.",
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Введите команду в формате: /getuser {Имя}",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
