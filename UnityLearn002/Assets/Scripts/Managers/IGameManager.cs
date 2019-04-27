public interface IGameManager
{
    ManagerStatus Status { get; }
    void Startup(NetworkService service);
}

public enum ManagerStatus
{
    Shutdown,
    Initializing,
    Started
}
