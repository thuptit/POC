namespace POC.MiniIoC;

public interface ILifetime : IScope
{
    object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory);
    object GetServicePerScope(Type type, Func<ILifetime, object> factory);
}