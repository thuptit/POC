namespace POC.MiniIoC;

public class ContainerLifeTime : ObjectCache , ILifetime
{
    public Func<Type, Func<ILifetime,object>> GetFactory { get; private set; }
    public ContainerLifeTime(Func<Type, Func<ILifetime, object>> getFactory) => 
        GetFactory = getFactory;
    
    public object? GetService(Type serviceType) => GetFactory(serviceType)(this);

    public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
        => GetCache(type, factory, this);

    public object GetServicePerScope(Type type, Func<ILifetime, object> factory) =>
        GetServiceAsSingleton(type, factory);
}