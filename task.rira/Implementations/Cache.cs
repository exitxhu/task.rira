using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using task.rira.Abstractions;
using task.rira.sdk;

namespace task.rira.Implementations;

public class PersonCache(IMemoryCache cache) : ICache<PersonDto>
{
    public Task Delete(string key)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<PersonDto> Get(string key)
    {
        cache.TryGetValue<PersonDto>(key, out var result);
        return Task.FromResult(result);
    }

    public Task Set(string key, PersonDto value)
    {
        cache.Set<PersonDto>(key, value);
        return Task.CompletedTask;
    }
}
