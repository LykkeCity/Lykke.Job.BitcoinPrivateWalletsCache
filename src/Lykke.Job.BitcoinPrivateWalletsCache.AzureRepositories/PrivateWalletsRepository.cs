using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Repositories;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Job.BitcoinPrivateWalletsCache.AzureRepositories
{
    public class PrivateWalletEntity : TableEntity, IPrivateWallet
    {
        public string ClientId { get; set; }
        public string WalletAddress { get; set; }
        public string WalletName { get; set; }
        public string EncodedPrivateKey { get; set; }
        public bool? IsColdStorage { get; set; }
        public int? Number { get; set; }
        
        public static class ByClient
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }            
        }        
    }

    public class PrivateWalletsRepository : IPrivateWalletsRepository
    {
        private readonly INoSQLTableStorage<PrivateWalletEntity> _tableStorage;

        public PrivateWalletsRepository(INoSQLTableStorage<PrivateWalletEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<IEnumerable<IPrivateWallet>> GetStoredWallets(string clientId)
        {
            var partition = PrivateWalletEntity.ByClient.GeneratePartitionKey(clientId);
            return await _tableStorage.GetDataAsync(partition);
        }
    }
}
