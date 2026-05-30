using Microsoft.Extensions.Caching.Distributed;
using task.rira.sdk;

namespace task.rira.Abstractions;

public interface ICache<T>
{
    Task<T> Get(string key);
    Task Set(string key, T value);
    Task Delete(string key);

}
public interface IPersonRepo
{
    Task<PersonDto> Find(int id);
    Task<int> Insert(PersonDto value);
    Task<bool> Update(PersonDto value);//maybe other values
    Task Delete(int id);

}
public interface IQueue
{
    object? Dequeue(string queue);
    void Enqueue(string queue, object val);
    bool TryDequeue(string queue, out object? val);

}