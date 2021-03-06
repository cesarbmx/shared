﻿using System.IO;
using System.Reflection;
using CesarBmx.Shared.Application.Settings;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CesarBmx.Shared.Common.Extensions;

namespace CesarBmx.Shared.Api.Configuration
{
    public static class Log4NetConfig
    {
        public static ILoggerFactory ConfigureSharedLog4Net(this ILoggerFactory logger, Assembly assembly, IHostEnvironment environment, IConfiguration configuration)
        {
            // Grab AppSettings
            var appSettings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettings);

            var logRepository = LogManager.GetRepository(assembly);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // Log version number
            GlobalContext.Properties["version"] = assembly.VersionNumber();
            GlobalContext.Properties["environment"] = environment.EnvironmentName;
            GlobalContext.Properties["applicationId"] = appSettings.ApplicationId;

            logger.AddLog4Net();

            return logger;
        }
    }
}
