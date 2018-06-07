using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Services;
using Lykke.Service.Session.Client;

namespace Lykke.Job.BitcoinPrivateWalletsCache.PeriodicalHandlers
{
    public class CacheWarmUp : TimerPeriod
    {
        private readonly IClientSessionsClient _sessionsClient;
        private readonly ILog _log;
        private readonly IList<string> _excludeList;
        private readonly IBalancePoller _balancePoller;
        private static bool _inProcess;

        public CacheWarmUp(IClientSessionsClient sessionsClient, ILog log, TimeSpan timerPeriod, IList<string> excludeList, IBalancePoller balancePoller) : base(nameof(CacheWarmUp), (int)timerPeriod.TotalMilliseconds, log)
        {
            _sessionsClient = sessionsClient;
            _log = log;
            _excludeList = excludeList;
            _balancePoller = balancePoller;
        }

        public override async Task Execute()
        {
            if (_inProcess)
                return;

            try
            {
                _inProcess = true;

                var timestamp = DateTime.UtcNow;
                var clientsIds = (await _sessionsClient.GetActiveClientIdsAsync()).Where(id => !_excludeList.Contains(id)).ToList();

                await _log.WriteInfoAsync(GetComponentName(), "Updating cache", $"Processing {clientsIds.Count} active clients.");
                foreach (var clientId in clientsIds)
                {
                    await _balancePoller.WarmUp(clientId).ConfigureAwait(false);
                }

                await _log.WriteInfoAsync(GetComponentName(), "Updating cache", $"Processed in {(DateTime.UtcNow - timestamp).TotalSeconds} seconds.");
            }
            finally
            {

                _inProcess = false;
            }
        }
    }
}
