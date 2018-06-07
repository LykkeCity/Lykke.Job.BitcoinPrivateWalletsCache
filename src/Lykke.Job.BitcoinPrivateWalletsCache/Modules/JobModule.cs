using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Job.BitcoinPrivateWalletsCache.AzureRepositories;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Repositories;
using Lykke.Job.BitcoinPrivateWalletsCache.Core.Services;
using Lykke.Job.BitcoinPrivateWalletsCache.Services;
using Lykke.SettingsReader;
using Lykke.Job.BitcoinPrivateWalletsCache.PeriodicalHandlers;
using Lykke.Job.BitcoinPrivateWalletsCache.Settings;
using Lykke.Service.Session.Client;
using Microsoft.Extensions.Hosting;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Modules
{
    public class JobModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settingsManager;

        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public JobModule(IReloadingManager<AppSettings> settingsManager, ILog log)
        {
            _settingsManager = settingsManager;
            _log = log;
            
            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();
            
            builder.RegisterInstance<IPrivateWalletsRepository>(
                new PrivateWalletsRepository(AzureTableStorage<PrivateWalletEntity>.Create(_settingsManager.ConnectionString(x => x.BitcoinPrivateWalletsCacheJob.Db.ClientPersonalInfoConnString), "PrivateWallets", _log)));

            builder.RegisterType<BalancePoller>().As<IBalancePoller>().SingleInstance();

            builder.RegisterType<BalanceSubscriber>().As<IHostedService>().SingleInstance()
                .WithParameter("attemptDelay", _settingsManager.CurrentValue.BitcoinPrivateWalletsCacheJob.AttemptDelay)
                .WithParameter("attemptsCount", _settingsManager.CurrentValue.BitcoinPrivateWalletsCacheJob.AttemptsCount);

            builder.RegisterType<NinjaFacade>().As<INinjaFacade>().SingleInstance()
                .WithParameter("ninjaUrl", _settingsManager.CurrentValue.BitcoinPrivateWalletsCacheJob.NinjaUrl);
            
            builder.RegisterClientSessionClient(_settingsManager.CurrentValue.SessionServiceClient.SessionServiceUrl, _log);

            RegisterPeriodicalHandlers(builder);
            
            builder.Populate(_services);
        }

        private void RegisterPeriodicalHandlers(ContainerBuilder builder)
        {
            builder.RegisterType<CacheWarmUp>()
                .WithParameter("timerPeriod", _settingsManager.CurrentValue.BitcoinPrivateWalletsCacheJob.MainLoopTimerPeriod)
                .WithParameter("excludeList", _settingsManager.CurrentValue.BitcoinPrivateWalletsCacheJob.ExcludeClientIdList)
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
        }
    }
}
