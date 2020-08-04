using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            string webHookUrl = "https://hooks.slack.com/services/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";
            await new SlackClient(webHookUrl).SendAsync(b => b
                .WithUsername("Sigfox Bot")
                .WithText(message)
            );
        }
    }
}
