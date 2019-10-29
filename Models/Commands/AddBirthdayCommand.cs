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

            var admins = await botClient.GetChatAdministratorsAsync(chatId);
            if (admins.FirstOrDefault(a=>a.User.Id == message.From.Id) == null)
            {
                await botClient.SendTextMessageAsync(chatId, "Добавлять информацию может только администратор.",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

                return;
            }

            var str = message.Text.Split(new char[] { ' ' });

            if (str.Length >= 3)
            {
                DateTime birthday;

                if (DateTime.TryParse(str[str.Length - 1], out birthday))
                {
                    AppDbContext context = new AppDbContext();

                    string name = String.Empty;
                    for (int i = 1; i < str.Length - 1; i++)
                    {
                        name += str[i];

                        if (i > 1 && i < str.Length - 2)
                        {
                            name += " ";
                        }
                    }

                    await context.Create(
                        new User { Name = name, Birthday = birthday.ToLocalTime(), ChatId = chatId });

                    var m = "*Добавлено:* \n" + name + ": *" + birthday.Day + "." + birthday.Month + "*";
                    await botClient.SendTextMessageAsync(chatId, m,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId, "Введите команду в формате: /addbirthday {Имя} {Дата}",
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
