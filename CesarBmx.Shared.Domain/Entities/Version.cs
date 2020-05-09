﻿using System;

namespace CesarBmx.Shared.Domain.Entities
{
    public class Version
    {
        public string VersionNumber { get; private set; }
        public DateTime LastBuild { get; private set; }
        public string Environment { get; private set; }

        public Version(
            string versionNumber,
            DateTime lastBuildOccurred,
            string environment)
        {
            VersionNumber = versionNumber;
            LastBuild = lastBuildOccurred;
            Environment = environment;
        }
    }
}
