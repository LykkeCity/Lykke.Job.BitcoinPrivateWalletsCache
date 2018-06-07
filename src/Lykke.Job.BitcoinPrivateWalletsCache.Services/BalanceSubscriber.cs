using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain;
using Microsoft.Extensions.Hosting;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Services
{
    public class BalanceSubscriber : IHostedService
    {
        private readonly ILog _log;
        private readonly INinjaFacade _ninjaFacade;
        private readonly TimeSpan _attemptDelay;
        private readonly int _attemptsCount;
        private IDisposable _addressesObservable;
        
        public BalanceSubscriber(ILog log, INinjaFacade ninjaFacade, TimeSpan attemptDelay, int attemptsCount)
        {
            _log = log;
            _ninjaFacade = ninjaFacade;
            _attemptDelay = attemptDelay;
            _attemptsCount = attemptsCount;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _addressesObservable = Observable
                .FromEventPattern<BalanceNotCalculatedEventArgs>(
                    h => _ninjaFacade.BalanceNotCalculated += h,
                    h => _ninjaFacade.BalanceNotCalculated -= h).Delay(_attemptDelay).Subscribe(pattern =>
                {
                    if (pattern.EventArgs.AttemptsCount < _attemptsCount)
                    {
                        _log.WriteWarning("Ninja balance poller", pattern.EventArgs.ToJson(), "balance has not been calculated. Retry.");

                        _ninjaFacade.SendBalanceRequest(pattern.EventArgs.Address, pattern.EventArgs.AttemptsCount + 1);
                    }
                    else
                    {
                        _log.WriteError("Ninja balance poller", pattern.EventArgs);
                    }
                });
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _addressesObservable.Dispose();
            
            return Task.CompletedTask;
        }
    }
}
