namespace POC.MiniIoC;

public class ScopeLifetime : ObjectCache, ILifetime
{
    private readonly ContainerLifeTime _parentLifetime;

    public ScopeLifetime(ContainerLifeTime parentLifetime) => _parentLifetime = parentLifetime;

    public object? GetService(Type serviceType) => _parentLifetime.GetFactory(serviceType)(this);

    public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory) =>
        _parentLifetime.GetServiceAsSingleton(type, factory);

    public object GetServicePerScope(Type type, Func<ILifetime, object> factory) =>
        GetCache(type, factory,this);
}