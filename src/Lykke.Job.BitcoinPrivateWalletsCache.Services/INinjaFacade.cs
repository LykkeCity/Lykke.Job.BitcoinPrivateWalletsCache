using System;
using System.Threading.Tasks;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Services
{
    public interface INinjaFacade
    {
        event EventHandler<BalanceNotCalculatedEventArgs> BalanceNotCalculated;
        Task SendBalanceRequest(string address, int attempt = 1);
    }
}
