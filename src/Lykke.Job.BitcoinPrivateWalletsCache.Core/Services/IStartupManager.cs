﻿using System.Threading.Tasks;

namespace Lykke.Job.BitcoinPrivateWalletsCache.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}