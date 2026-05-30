using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using task.rira.Abstractions;
using task.rira.Data;
using task.rira.Globals;
using task.rira.sdk;

namespace task.rira.Services;

public class PersonService(ILogger<PersonService> logger, IQueue queue, IPersonRepo repo, AppDbContext ctx) : IPersonService
{
    public async ValueTask<gRpcResponse> Delete(PersonDeleteRequestDto person)
    {
        await repo.Delete(person.Id);
        return new()
        {
            InternalMessage = "ok",
            IsSucessful = true
        };
    }

    public async ValueTask<gRpcResponse<PersonDto?>> Get(PersonGetRequestDto person)
    {
        var res = await repo.Find(person.Id);
        return new()
        {
            Value = res,
            InternalMessage = "ok",
            IsSucessful = true
        };
    }

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

    public async ValueTask<gRpcResponse<PersonDto[]?>> Search(PersonSearchQueryDto person)
    {
        // for simplicity i done search here, but it should be encapsulated
        // this could or in my idea shoud perform on a seprated database
        var pers = ctx.People.AsQueryable();

        if (!string.IsNullOrEmpty(person.FirstName))
            pers = pers.Where(a => a.FirstName.Contains(person.FirstName));

        if (!string.IsNullOrEmpty(person.LastName))
            pers = pers.Where(a => a.LastName.Contains(person.LastName));

        if (!string.IsNullOrEmpty(person.NationalId))
            pers = pers.Where(a => a.NationalId.Contains(person.NationalId));

        if (person.BirthDateFrom.HasValue)
            pers = pers.Where(a => a.BirthDate > person.BirthDateFrom);

        if (person.BirthDateTo.HasValue)
            pers = pers.Where(a => a.BirthDate < person.BirthDateFrom);

        pers = pers.Skip((person.Page - 1) * person.Size).Take(person.Size);

        var res = await pers.Select(a => new PersonDto
        {
            BirthDate = a.BirthDate,
            FirstName = a.FirstName,
            Id = a.Id,
            LastName = a.LastName,
            NationalId = a.NationalId
        }).ToArrayAsync();

        return new()
        {
            InternalMessage = "some search has done",
            Value = res,
            IsSucessful = true
        };
    }

    public async ValueTask<gRpcResponse> Update(PersonDto person)
    {
        var res = await repo.Update(person);
        return new()
        {
            InternalMessage = "ok",
            IsSucessful = res
        };
    }
}
