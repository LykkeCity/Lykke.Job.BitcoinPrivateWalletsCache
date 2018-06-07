using System;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Settings.JobSettings
{
    public class BitcoinPrivateWalletsCacheSettings
    {
        public DbSettings Db { get; set; }
        public string NinjaUrl { get; set; }
        public TimeSpan MainLoopTimerPeriod { get; set; }        
        public string[] ExcludeClientIdList { get; set; }        
        public TimeSpan AttemptDelay { get; set; }
        public int AttemptsCount { get; set; }
    }    
}
