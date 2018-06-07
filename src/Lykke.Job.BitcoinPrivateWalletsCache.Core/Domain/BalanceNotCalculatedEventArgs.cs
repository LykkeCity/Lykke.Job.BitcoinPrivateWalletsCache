using System;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Core.Domain
{
    public class BalanceNotCalculatedEventArgs : EventArgs
    {
        public string Address { get; set; }
        public int AttemptsCount { get; set; }
    }
}
