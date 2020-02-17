using Quartz;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayBot.Models
{
    [DisallowConcurrentExecution]
    public class WriteJob : IJob
    {
        public WriteJob()
        {
        }

        public async Task Execute(IJobExecutionContext context)
        {
            AppDbContext cont = new AppDbContext();
            var botClient = await Bot.GetBotClientAsync();

            var groups = cont.GetChats();

            foreach (var group in groups)
            {
                var result = await cont.GetUsers(String.Empty, group);
                var items = result.Where(i => i.Birthday.Day == DateTime.Today.Day && i.Birthday.Month == DateTime.Today.Month);

                if (items.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder("*Дни рождения сегодня:*\n");
                    foreach (var i in items)
                    {
                        sb.AppendFormat("{0}\n", i.Name);
                    }

                    await botClient.SendTextMessageAsync(group, sb.ToString(),
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }

                var itemsweek = result.Where(i => i.Birthday.Date.AddYears(-i.Birthday.Year + 1) == DateTime.Today.AddDays(7).AddYears(-DateTime.Today.Year + 1));

                if (itemsweek.Count() > 0)
                {
                    StringBuilder sb = new StringBuilder("*Через неделю День рождения у:*\n");
                    foreach (var i in itemsweek)
                    {
                        sb.AppendFormat("{0} ({1}.{2})\n", i.Name, i.Birthday.Day, i.Birthday.Month);
                    }

                    await botClient.SendTextMessageAsync(group, sb.ToString(),
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
        }
    }
}
