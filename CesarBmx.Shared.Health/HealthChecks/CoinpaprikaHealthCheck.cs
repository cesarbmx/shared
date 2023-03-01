using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CesarBmx.Shared.Health.HealthChecks
{
    public class CoinpaprikaHealthCheck : IHealthCheck
    {
        private readonly CoinpaprikaAPI.Client _coinpaprikaClient;


        public CoinpaprikaHealthCheck(CoinpaprikaAPI.Client coinpaprikaClient)
        {
            _coinpaprikaClient = coinpaprikaClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var response = await _coinpaprikaClient.GetClobalsAsync();

                if (response.Value == null) return HealthCheckResult.Degraded("https://api.coinpaprika.com/");
                return HealthCheckResult.Healthy("https://api.coinpaprika.com/");

            }
            catch (Exception ex)
            {
                // Return result
                return HealthCheckResult.Unhealthy(ex.Message);
            }
        }
    }
}