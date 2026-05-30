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


public class QueueService : IQueue
{
    private readonly Dictionary<string, Queue<object>> _queues = new();

    public void Enqueue(string queue, object val)
    {

        if (!_queues.TryGetValue(queue, out var q))
        {
            q = new Queue<object>();

            _queues[queue] = q;
        }

        q.Enqueue(val);
    }
    public object? Dequeue(string queue)
    {
        if (!_queues.TryGetValue(queue, out var q))
            return null;

        if (q.Count == 0)
            return null;

        return q.Dequeue();
    }

    public bool TryDequeue(string queue, out object? val)
    {
        val = null;
        if (!_queues.TryGetValue(queue, out var q) || q.Count == 0)
            return false;
        val = q.Dequeue();
        return true;
    }
}
