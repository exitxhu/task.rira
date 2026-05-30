using Grpc.Net.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProtoBuf.Grpc.Client;
using task.rira.sdk;
using Polly;


namespace task.rira.client.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    //can be placed somewhere better or as singletone service
    static ResiliencePipeline breaker = new ResiliencePipelineBuilder()
        .AddTimeout(TimeSpan.FromSeconds(5))
        .AddCircuitBreaker(new()
        {
            FailureRatio = 0.5,
            SamplingDuration = TimeSpan.FromSeconds(30),
            BreakDuration = TimeSpan.FromSeconds(30)
        })
        .Build();

    string letters = "abcdefghijklmnopqrstuvwxyz";
    string numbers = "0123456789";
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<object> Get()
    {

        using (var channel = GrpcChannel.ForAddress("https://localhost:10042"))
        {
            try
            {
                return await breaker.ExecuteAsync(async ct =>
                {
                    using var channel =
                        GrpcChannel.ForAddress("https://localhost:10042");

                    var service =
                        channel.CreateGrpcService<IPersonService>();

                    return await service.InsertSingle(new PersonDto()
                    {
                        BirthDate = new DateTime(Random.Shared.Next(1900, 2026),
                            Random.Shared.Next(1, 12),
                            Random.Shared.Next(1, 28)),
                        FirstName = Enumerable.Range(0, Random.Shared.Next(3, 9))
                            .Select(a => letters[Random.Shared.Next(0, letters.Length - 1)]).ToString()!,
                        LastName = Enumerable.Range(0, Random.Shared.Next(3, 9))
                            .Select(a => letters[Random.Shared.Next(0, letters.Length - 1)]).ToString()!,
                        NationalId = Enumerable.Range(0, 10)
                            .Select(a => numbers[Random.Shared.Next(0, numbers.Length - 1)]).ToString()!
                    });
                });
            }
            catch
            {
                // persist the msg like this maybe?:
                // failedRepo.add(PersonDto);

                throw;
            }
        }
    }
}
