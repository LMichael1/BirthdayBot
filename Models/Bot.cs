using BirthdayBot.Models.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace BirthdayBot.Models
{
    public class Bot
    {
        private static TelegramBotClient botClient;
        private static List<ICommand> commandsList;

        public static IReadOnlyList<ICommand> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient != null)
            {
                return botClient;
            }

            commandsList = new List<ICommand>();
            commandsList.Add(new StartCommand());
            commandsList.Add(new AddBirthdayCommand());
            commandsList.Add(new TodayCommand());
            //TODO: Add more commands

            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/update");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
