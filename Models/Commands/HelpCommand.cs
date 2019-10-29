using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public class HelpCommand : ICommand
    {
        public string Name => @"/help";

        public bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(this.Name);
        }

        public async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;

            var text = "*Список команд:*\n/today - Дни рождения сегодня\n/week - Дни рождения в ближайшую неделю\n/month - Дни рождения в ближайший месяц\n/all - Все даты рождения\n/getuser {Имя} - Получить информацию о пользователе\n/help - Справка";
            await botClient.SendTextMessageAsync(chatId, text, 
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
