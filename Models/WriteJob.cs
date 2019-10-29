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

            var result = await cont.GetUsers(String.Empty, -1001191153807);
            var items = result.Where(i => i.Birthday.Date >= DateTime.Today &&
                                i.Birthday.Date <= DateTime.Today.AddDays(30));

            if (items.Count() > 0)
            {
                StringBuilder sb = new StringBuilder("*Дни рождения в ближайший месяц:*\n");
                foreach (var i in items)
                {
                    sb.AppendFormat("{0}: *{1}.{2}*\n", i.Name, i.Birthday.Day, i.Birthday.Month);
                }

                await botClient.SendTextMessageAsync(-1001191153807, sb.ToString(),
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            else
            {
                await botClient.SendTextMessageAsync(-1001191153807, "Именинники не найдены.",
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
        }
    }
}
