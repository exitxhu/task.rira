using System.Text.Json;
using System.Text.Json.Serialization;
using task.rira.Abstractions;
using task.rira.Globals;
using task.rira.sdk;

namespace task.rira.Services;

public class PersonService(ILogger<PersonService> logger, IQueue queue) : IPersonService
{
    public async ValueTask<gRpcResponse> InsertBatch(PersonDto[] people)
    {
        logger.LogCritical(JsonSerializer.Serialize(people));
        foreach (var person in people)
        {
            queue.Enqueue(Constants.PersonInsertQueue, person);
        }
        return new()
        {
            InternalMessage = "ok",
            IsSucessful = true
        };
    }

    public async ValueTask<gRpcResponse> InsertSingle(PersonDto person)
    {
        logger.LogCritical(JsonSerializer.Serialize(person));
        queue.Enqueue(Constants.PersonInsertQueue, person);
        return new()
        {
            InternalMessage = "ok",
            IsSucessful = true
        };

    }
}
