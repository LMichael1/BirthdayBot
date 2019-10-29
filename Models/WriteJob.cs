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
                var items = result.Where(i => i.Birthday.Date == DateTime.Today);

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
                else
                {
                    await botClient.SendTextMessageAsync(group, "Именинники не найдены.",
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
        }
    }
}
