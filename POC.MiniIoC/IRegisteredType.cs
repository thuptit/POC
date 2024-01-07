namespace POC.MiniIoC;

public interface IRegisteredType
{
    void AsSingleton();
    void PerScope();
}