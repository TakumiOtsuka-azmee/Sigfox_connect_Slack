using System;
using System.Timers;

namespace Sigfox_connect_Slack
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Program Start ==========");
            Console.WriteLine("wait 60sec");
            Timer timer = new Timer(60000);
            timer.Enabled = true;

            long beforePushTime = 0;

            timer.Elapsed += async (sender, e) =>
            {
                var sigfoxData = await SigfoxFunction.GetMessageFromSigfox();
                long pushTimeUnix = (long)sigfoxData["data"][0]["time"];
                Console.WriteLine(pushTimeUnix);
                Console.WriteLine(beforePushTime);

                // Notify Slack of the temperature data 
                // if the time of the last data obtained and the time of the data obtained this time do not match.
                if (beforePushTime != pushTimeUnix)
                {
                    // time
                    DateTimeOffset pushTime = DateTimeOffset.FromUnixTimeMilliseconds(pushTimeUnix).ToOffset(new TimeSpan(9, 0, 0));

                    Console.WriteLine(pushTime);
                    beforePushTime = pushTimeUnix;

                    // data 20bit
                    // detail
                    // Count: 4bit, Temperature: 8bit, Voltage: 8bit
                    string pushData = sigfoxData["data"][0]["data"].ToString();         // The most recent data is always in the 0s.
                    Console.WriteLine("Temperature: {0}", SigfoxFunction.hex2float(pushData.Substring(4, 8)));
                    Console.WriteLine("Voltage: {0}", SigfoxFunction.hex2float(pushData.Substring(12, 8)));

                    string message = ":thermometer: 現在のオフィスの気温は" + SigfoxFunction.hex2float(pushData.Substring(4, 8)).ToString() + "℃です " +
                                    "(Sensor push time: " + pushTime + ")\n";

                    await Slack.SlackPost(message);
                }
            };

            Console.ReadLine();
        }
    }
}
