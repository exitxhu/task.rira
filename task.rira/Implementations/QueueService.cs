using task.rira.Abstractions;

namespace task.rira.Implementations;

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
