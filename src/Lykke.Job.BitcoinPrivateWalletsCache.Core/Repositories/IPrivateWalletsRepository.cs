using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Core.Repositories
{
    public interface IPrivateWalletsRepository
    {
        /// <summary>
        /// Returns all stored wallets, except default.
        /// To get all use extension GetPrivateWallet
        /// </summary>
        /// <param name="clientId">client id</param>
        /// <returns>Private wallets enumeration</returns>
        Task<IEnumerable<IPrivateWallet>> GetStoredWallets(string clientId);
    }
}
