using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace PostBot
{
    public static class BaseJob:object
    {
        private static readonly long ChatId = Convert.ToInt64(ConfigurationManager.AppSettings["ChatId"]);
        private static readonly string ApiToken = ConfigurationManager.AppSettings["ApiToken"];
        private static TelegramBotClient Bot = new TelegramBotClient(ApiToken);
    }
}
