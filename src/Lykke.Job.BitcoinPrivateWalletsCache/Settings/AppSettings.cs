using Lykke.Job.BitcoinPrivateWalletsCache.Settings.JobSettings;
using Lykke.Job.BitcoinPrivateWalletsCache.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Settings
{
    public class AppSettings
    {
        public BitcoinPrivateWalletsCacheSettings BitcoinPrivateWalletsCacheJob { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        [Optional]
        public MonitoringServiceClientSettings MonitoringServiceClient { get; set; }

        public SessionServiceClientSettings SessionServiceClient { get; set; }
    }

    public class SessionServiceClientSettings
    {
        public string SessionServiceUrl { get; set; }
    }
}
