using Azure.Core;
using Microsoft.Identity.Abstractions;

namespace Auths.Web;

public class WeatherApiClient(/*HttpClient httpClient,*/ IDownstreamApi _downstreamApi)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<WeatherForecast>? forecasts = null;

        var response = await _downstreamApi.GetForUserAsync<WeatherForecast[]>("AuthsApi", options =>
        {
            options.RelativePath = "/weatherforecast";
        },
        cancellationToken: cancellationToken);

        /*await foreach (var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }*/

        if (response != null) { 
            foreach (var forecast in response)
            {
                if (forecasts?.Count >= maxItems)
                {
                    break;
                }
                if (forecast is not null)
                {
                    forecasts ??= [];
                    forecasts.Add(forecast);
                }
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
