using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SlackWebhook;

namespace Sigfox_connect_Slack
{
    /// <summary>
    /// Slack bot
    /// </summary>
    class Slack
    {
        public static async Task SlackPost(string message)
        {
            string webHookUrl = getSlackInfo("SlackWebHook");
            await new SlackClient(webHookUrl).SendAsync(b => b
                .WithUsername("Sigfox Bot")
                .WithText(message)
            );
        }

        public static string getSlackInfo(string infoItem)
        {
            // Read Sigfox Information
            using (var sr = new StreamReader(@"../../../SlackInfo.json", Encoding.UTF8))
            {
                var sigfoxinfo = JObject.Parse(sr.ReadToEnd());

                return (string)sigfoxinfo[infoItem];
            }
        }
    }
}
