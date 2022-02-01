using System;
using System.Reflection;
using CesarBmx.Shared.Common.Extensions;
using Version = CesarBmx.Shared.Application.Responses.Version;

namespace CesarBmx.Shared.Application.ResponseBuilders
{
    public static class VersionResponseBuilder
    {
        public static Version Build(this Version versionResponse, Assembly assembly)
        {
            var assemblyDate = assembly.Date();
            versionResponse.BuildDateTime = assemblyDate.ToString("yyyy/MM/dd HH:mm");
            versionResponse.LastBuildOccurred = assemblyDate.DaysHoursMinutesAndSecondsSinceDate(DateTime.Now);
            versionResponse.VersionNumber = assembly.VersionNumber();

            return versionResponse;
        }
    }
}
