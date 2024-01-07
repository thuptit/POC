using POC.MiniIoC;

var container = new Container();
container.Register<ITest>(typeof(Test));
container.Register<ITest1>(typeof(Test1));
container.Register<IFoo>(typeof(Foo));

var foo = container.Resolve<IFoo>();
foo.Log();

public interface IFoo
{
    void Log();
}

public class Foo : IFoo
{
    public Foo(ITest test)
    {
    }
    public void Log()
    {
        Console.WriteLine("Hello");
    }
}

public interface ITest
{
}

public class Test : ITest
{
    public Test(ITest1 test1)
    {
    }
}
public interface ITest1{}
public class Test1 : ITest1{}