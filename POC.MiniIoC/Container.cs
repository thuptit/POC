using System.Linq.Expressions;
using System.Reflection;

namespace POC.MiniIoC;

public class Container : IScope
{
    /// <summary>
    /// Dictionary for manage types - instance of type
    /// </summary>
    private readonly Dictionary<Type, Func<ILifetime, object>> _registeredTypes = new();
    /// <summary>
    /// Lifetime Management
    /// </summary>
    private readonly ContainerLifeTime _lifeTime;

    public Container()
    {
        _lifeTime = new ContainerLifeTime(t => _registeredTypes[t]);
    }

    public IRegisteredType Register(Type @interface, Func<object> factory)
        => RegisterType(@interface, _ => factory());

    public IRegisteredType Register(Type @interface, Type impl)
        => RegisterType(@interface, FactoryFromType(impl));
    
    public object? GetService(Type serviceType)
    {
        Func<ILifetime, object> registeredType;
        if (!_registeredTypes.TryGetValue(serviceType, out registeredType))
        {
            return null;
        }

        return registeredType(_lifeTime);
    }

    private IRegisteredType RegisterType(Type itemType, Func<ILifetime, object> factory)
        => new RegisteredType(itemType, f => _registeredTypes[itemType] = f, factory);

    private static Func<ILifetime,object> FactoryFromType(Type itemType)
    {
        var constructors = itemType.GetConstructors();
        if (constructors.Length == 0)
        {
            constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        }

        var constructor = constructors.First();

        var arg = Expression.Parameter(typeof(ILifetime));
        var body = Expression.New(constructor, constructor.GetParameters().Select(param =>
        {
            var resolve = new Func<ILifetime, object>(lifetime => lifetime.GetService(param.ParameterType));
            return Expression.Convert(
                Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                param.ParameterType
            );
        }));
        var lambda = Expression.Lambda(
            body,
            arg
        );
        return (Func<ILifetime, object>)lambda.Compile();
    }
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}