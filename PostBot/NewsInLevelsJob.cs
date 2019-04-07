using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace PostBot
{
    public static class NewsInLevelsJob
    {
        private static readonly string ApiToken = ConfigurationManager.AppSettings["ApiToken"];
        private static readonly long ChatId = Convert.ToInt64(ConfigurationManager.AppSettings["ChatId"]);
        private static TelegramBotClient Bot = new TelegramBotClient(ApiToken);
        private static bool _runningFlag;
        private static readonly Semaphore RunningSemaphore = new Semaphore(1, 1);
        private static int _interval = Convert.ToInt32(ConfigurationManager.AppSettings["NewsInLevelsIntervalMins"]) * 60000;
        private static DateTime _startTime = Convert.ToDateTime(ConfigurationManager.AppSettings["NewsInLevelsJobStartTime"]);
        private static readonly Timer Timer = new Timer(DoWork, null, Timeout.Infinite, _interval);

        static NewsInLevelsJob()
        {
            Console.WriteLine("NewsInLevelsJob Timer created");
        }

        public static void Start()
        {
            var now = DateTime.Now;
            var startTime = _startTime;
            if (TimeSpan.FromMilliseconds(_interval).TotalDays >= 1)
            {
                var today = now.Date.AddMinutes(startTime.TimeOfDay.TotalMinutes);
                startTime = now <= today ? today : today.AddDays(1);
            }
            else
            {
                var dayIntervalMinutes = TimeSpan.FromMilliseconds(_interval).TotalMinutes;
                if (now > startTime)
                    do startTime = startTime.AddMinutes(dayIntervalMinutes); while (startTime < now);
                else
                {
                    var timeSpan = new TimeSpan(0, int.Parse(dayIntervalMinutes.ToString(CultureInfo.InvariantCulture)),
                        0);
                    while (startTime > now && startTime.Subtract(now).TotalMinutes >= dayIntervalMinutes)
                        startTime = startTime.Subtract(timeSpan);
                }
            }
            var dueTime = (int)(startTime - DateTime.Now).TotalMilliseconds;
            Timer.Change(dueTime, _interval);
            Console.WriteLine("NewsInLevelsJob job timer startTime is:" + startTime);
        }

        public static void Stop()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            Console.WriteLine("NewsInLevelsJob timer stopped");
        }

        private static void Restart()
        {
            Console.WriteLine("Try to restart NewsInLevelsJob job.");
            Stop();
            Start();
            Console.WriteLine("End of Restarting NewsInLevelsJob job.");
        }

        private static void DoWork(object state)
        {
            try
            {
                try
                {
                    Program.CrossJobMutex.WaitOne();
                    if (!_runningFlag)
                    {
                        RunningSemaphore.WaitOne();
                        _runningFlag = true;
                        RunningSemaphore.Release();
                    }
                    else
                        throw new Exception("Another Thread is Running !");

                    DoPost();

                    RunningSemaphore.WaitOne();
                    _runningFlag = false;
                    RunningSemaphore.Release();
                }
                finally
                {
                    Program.CrossJobMutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message != "Another Thread is Running !")
                {
                    RunningSemaphore.WaitOne();
                    _runningFlag = false;
                    RunningSemaphore.Release();
                }
            }
        }

        public static void DoPost()
        {
            try
            {
                Console.WriteLine($"try to put newsinlevels entry ,time: {DateTime.Now}");

                string html = Utility.GetHtml("https://www.newsinlevels.com");
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                var firstpageMainContent = htmlDoc.DocumentNode.SelectNodes(@"//div[contains(@class,'main-content')]");
                var topEntry = firstpageMainContent.Descendants().Where(a => a.HasClass("news-block")).Skip(1).FirstOrDefault();
                var imgwrap = topEntry.Descendants().FirstOrDefault(a => a.HasClass("img-wrap"));
                var anchor = imgwrap.Descendants("a").FirstOrDefault();
                var href = anchor.Attributes.FirstOrDefault(a => a.Name == "href").Value;

                var image = imgwrap.Descendants("img").FirstOrDefault();
                var imageSrc = image.Attributes.FirstOrDefault(a => a.Name == "src").Value;
                Utility.SendPhotoToChannel(ChatId, imageSrc, "\U0001F4A1" + "News In Levles\n#EnglishWithSamet \n #NewsInLevels").GetAwaiter().GetResult();

                (string title, string conent) level1 = GetArticleContent(href);
                Bot.SendTextMessageAsync(ChatId, "<b>"+level1.title+"</b>" + Environment.NewLine + level1.conent + Environment.NewLine + "#EnglishWithSamet\n#NewsInLevelsLevel1", ParseMode.Html).GetAwaiter().GetResult();

                (string title, string conent) level2 = GetArticleContent(href.Replace("level-1", "level-2"));
                Bot.SendTextMessageAsync(ChatId, "<b>" + level2.title + "</b>" + Environment.NewLine + level2.conent + Environment.NewLine + "#EnglishWithSamet\n#NewsInLevelsLevel2", ParseMode.Html).GetAwaiter().GetResult();

                (string title, string conent) level3 = GetArticleContent(href.Replace("level-1", "level-3"));
                Bot.SendTextMessageAsync(ChatId, "<b>" + level3.title + "</b>" + Environment.NewLine + level3.conent + Environment.NewLine + "#EnglishWithSamet\n#NewsInLevelsLevel3", ParseMode.Html).GetAwaiter().GetResult();

                Console.WriteLine($"End of putting newsinlevels entry ,time: {DateTime.Now}");

                if (Program.JobsRunOnce)
                    Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"an exception occured! -> {ex.Message}");
            }
        }

        public static (string title, string conent) GetArticleContent(string href)
        {
            string html;
            HtmlDocument htmlDoc;
            html = Utility.GetHtml(href);
            htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var mainContent = htmlDoc.DocumentNode.SelectNodes(@"//div[contains(@class,'main-content')]");
            var titleText = mainContent.Descendants("div").FirstOrDefault(a => a.HasClass("article-title")).Descendants("h2").FirstOrDefault().InnerText;
            var nContent = mainContent.Descendants().FirstOrDefault(a => a.Id == "nContent");

            string articleText = "";
            foreach (HtmlNode p in nContent.Descendants("p"))
            {
                articleText += p.InnerText + "\n";
            }
            var result = (title: titleText, conent: articleText);
            return result;
        }

      
    }
}


