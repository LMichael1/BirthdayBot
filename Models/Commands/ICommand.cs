using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BirthdayBot.Models.Commands
{
    public interface ICommand
    {
        string Name { get; }

        Task Execute(Message message, TelegramBotClient client);

        bool Contains(Message message);
    }
}
