using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Services
{
    public class NinjaFacade : INinjaFacade
    {
        private readonly ILog _log;
        private readonly string _ninjaUrl;
        
        public NinjaFacade(ILog log, string ninjaUrl)
        {
            _log = log;
            _ninjaUrl = ninjaUrl;
        }

        public event EventHandler<BalanceNotCalculatedEventArgs> BalanceNotCalculated;

        public async Task SendBalanceRequest(string address, int attempt = 1)
        {
            try
            {
                await DoRequest($"{_ninjaUrl}balances/{address}/summary?colored=true");                
            }
            catch (WebException ex)
            {
                _log.WriteWarning(nameof(NinjaFacade), new { Address = address, StatusCode = ex.Status.ToString(), ex.Message }, "Balance not calculated", ex);

                BalanceNotCalculated?.Invoke(this, new BalanceNotCalculatedEventArgs { Address = address, AttemptsCount = attempt });
            }
        }

        private static async Task<string> DoRequest(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            var webResponse = await webRequest.GetResponseAsync();
            using (var receiveStream = webResponse.GetResponseStream())
            {
                using (var sr = new StreamReader(receiveStream))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }
    }
}
