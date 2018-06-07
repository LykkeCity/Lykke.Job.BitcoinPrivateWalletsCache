using System.Linq;
using System.Threading.Tasks;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Repositories;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Services;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Services
{
    public class BalancePoller : IBalancePoller
    {
        private readonly INinjaFacade _ninjaFacade;
        private readonly IPrivateWalletsRepository _privateWalletsRepository;

        public BalancePoller(INinjaFacade ninjaFacade, IPrivateWalletsRepository privateWalletsRepository)
        {
            _ninjaFacade = ninjaFacade;            
            _privateWalletsRepository = privateWalletsRepository;            
        }
        
        public async Task WarmUp(string clientId)
        {
            var wallets = await _privateWalletsRepository.GetStoredWallets(clientId);

            foreach (var wallet in wallets)
            {
                await _ninjaFacade.SendBalanceRequest(wallet.WalletAddress);
            }
        }                
    }
}
