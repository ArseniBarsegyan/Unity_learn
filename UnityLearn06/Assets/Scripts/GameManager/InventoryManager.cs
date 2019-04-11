using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private List<string> _items;

    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        _items = new List<string>();
        Status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("Items: ");
        foreach (string item in _items)
        {
            builder.Append(item);
            builder.Append(" ");
        }
        Debug.Log(builder.ToString());
    }

    public void AddItem(string name)
    {
        _items.Add(name);
        DisplayItems();
    }
}
