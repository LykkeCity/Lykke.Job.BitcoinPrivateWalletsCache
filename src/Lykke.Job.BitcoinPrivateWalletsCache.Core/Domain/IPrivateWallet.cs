namespace Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain
{
    public interface IPrivateWallet
    {
        string ClientId { get; }
        string WalletAddress { get; }
        string WalletName { get; }
        string EncodedPrivateKey { get; set; }        
        bool? IsColdStorage { get; set; }
        int? Number { get; set; }
    }
}
