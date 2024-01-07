using System.Collections.Concurrent;

namespace POC.MiniIoC;

public abstract class ObjectCache
{
    private readonly ConcurrentDictionary<Type, object> _instanceCache = new();

    protected object GetCache(Type type, Func<ILifetime, object> factory, ILifetime lifetime)
        => _instanceCache.GetOrAdd(type, _ => factory(lifetime));

    public void Dispose()
    {
        foreach (var obj in _instanceCache.Values)
        {
            (obj as IDisposable)?.Dispose();
        }
    }
}