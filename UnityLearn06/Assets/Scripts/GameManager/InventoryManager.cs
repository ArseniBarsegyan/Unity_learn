using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        Status = ManagerStatus.Started;
    }
}
