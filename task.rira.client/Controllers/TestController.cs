using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Polly;
using ProtoBuf.Grpc.Client;
using task.rira.sdk;

namespace task.rira.client.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
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

    private IPersonService CreateService()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:10042");

        return channel.CreateGrpcService<IPersonService>();
    }

    private async Task<T> Execute<T>(Func<IPersonService, ValueTask<T>> action)
    {
        return await breaker.ExecuteAsync(async ct =>
        {
            var service = CreateService();

            return await action(service);
        });
    }

    private PersonDto RandomPerson()
    {
        return new PersonDto
        {
            BirthDate = new DateTime(
                Random.Shared.Next(1900, 2026),
                Random.Shared.Next(1, 12),
                Random.Shared.Next(1, 28)),

            FirstName = new string(
                Enumerable.Range(0, Random.Shared.Next(3, 9))
                    .Select(_ => letters[Random.Shared.Next(letters.Length)])
                    .ToArray()),

            LastName = new string(
                Enumerable.Range(0, Random.Shared.Next(3, 9))
                    .Select(_ => letters[Random.Shared.Next(letters.Length)])
                    .ToArray()),

            NationalId = new string(
                Enumerable.Range(0, 10)
                    .Select(_ => numbers[Random.Shared.Next(numbers.Length)])
                    .ToArray())
        };
    }

    [HttpPost("sample")]
    public Task<gRpcResponse> InsertSamplePerson()
        => Execute(x => x.InsertSingle(RandomPerson()));

    [HttpPost("insert")]
    public Task<gRpcResponse> Insert(PersonDto person)
        => Execute(x => x.InsertSingle(person));

    [HttpPost("batch/{count:int}")]
    public Task<gRpcResponse> InsertBatch(int count)
        => Execute(x => x.InsertBatch(
            Enumerable.Range(0, count)
                .Select(_ => RandomPerson())
                .ToArray()));

    [HttpPut]
    public Task<gRpcResponse> Update(PersonDto person)
        => Execute(x => x.Update(person));

    [HttpDelete]
    public Task<gRpcResponse> Delete(PersonDeleteRequestDto person)
        => Execute(x => x.Delete(person));

    [HttpGet("{id}")]
    public Task<gRpcResponse<PersonDto?>> Get(int id)
        => Execute(x => x.Get(new PersonGetRequestDto
        {
            Id = id
        }));

    [HttpPost("search")]
    public Task<gRpcResponse<PersonDto[]?>> Search(
        PersonSearchQueryDto query)
        => Execute(x => x.Search(query));
}