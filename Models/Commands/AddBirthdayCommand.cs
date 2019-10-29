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

            var str = message.Text.Split(new char[] { ' ' });

            if (str.Length >= 3)
            {
                if (DateTime.TryParse(str[str.Length - 1], out _))
                {
                    AppDbContext context = new AppDbContext();

                    System.Globalization.CultureInfo cultureinfo =
                            new System.Globalization.CultureInfo("uk-UA");
                    DateTime birthday =DateTime.Parse(str[str.Length - 1], cultureinfo);
                    string name = String.Empty;

                    for (int i = 1; i < str.Length - 1; i++)
                    {
                        name += str[i];
                    }

                    await context.Create(
                        new User { Name = name, Birthday = birthday });

                    var m = "Добавлено: " + name + " " + str[2];
                    await botClient.SendTextMessageAsync(chatId, m,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId, "Введите команду в формате: /addbirthday {Имя} {Дата}",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
