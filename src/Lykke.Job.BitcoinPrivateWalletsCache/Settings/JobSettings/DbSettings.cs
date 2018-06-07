using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Settings.JobSettings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string ClientPersonalInfoConnString { get; set; }
    }
}
