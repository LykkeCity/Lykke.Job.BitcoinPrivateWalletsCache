using System;
using System.Threading.Tasks;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Core.Services
{
    public interface IBalancePoller
    {
        Task WarmUp(string clientId);
    }
}
