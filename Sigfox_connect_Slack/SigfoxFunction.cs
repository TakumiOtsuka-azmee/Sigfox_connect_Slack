using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sigfox_connect_Slack
{
    class SigfoxFunction
    {
        /// <summary>
        /// Getting Data from the Sigfox Cloud API.
        /// </summary>
        /// <returns>return jObject data</returns>
        public static async Task<JObject> GetMessageFromSigfox()
        {
            // Get device information
            var sigfoxDeviceId = getSigfoxInfo("DeviceId");
            var loginId = getSigfoxInfo("loginId");
            var password = getSigfoxInfo("password");

            string messageUri = "https://api.sigfox.com/v2/devices/" + sigfoxDeviceId + "/messages";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(messageUri)
            };

            // Add Header for Basic Authorization
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                                            Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", loginId, password))));

            // Get data from Sigfox API.
            JObject jsonData;

            using (var httpClient = new HttpClient())
            {
                // Send a request to the API URI and check the response.
                var response = await httpClient.SendAsync(request);

                // If the authentication is successful, return the value obtained.
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Basic Authorization is Succeeded");
                    jsonData = JObject.Parse(await response.Content.ReadAsStringAsync());
                    return jsonData;
                }
                else
                {
                    Console.WriteLine("Basic Authorization Error");
                    return null;
                }
            }
        }

        /// <summary>
        /// Get information from SigfoxInfo.json
        /// items: "DeviceId", "loginId", "password"
        /// </summary>
        /// <param name="infoItem">The item name you want to get</param>
        /// <returns name = "(string)sigfoxinfo[infoItem]">The content of the item specified by the argument</returns>
        public static string getSigfoxInfo(string infoItem)
        {
            // Read Sigfox Information
            using (var sr = new StreamReader(@"../../../SigfoxInfo.json", Encoding.UTF8))
            {
                var sigfoxinfo = JObject.Parse(sr.ReadToEnd());

                return (string)sigfoxinfo[infoItem];
            }
        }

        /// <summary>
        /// HEX value convet to float value
        /// </summary>
        /// <param name="hexStr">Hex string</param>
        /// <returns>float value</returns>
        public static float hex2float(string hexStr)
        {
            float f;

            uint num = uint.Parse(hexStr, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] floatVals = BitConverter.GetBytes(num);

            return f = BitConverter.ToSingle(floatVals, 0);
        }
    }
}
