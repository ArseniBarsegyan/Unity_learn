using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private Dictionary<string, int> _items;

    public ManagerStatus Status { get; private set; }

    public void Startup()
    {
        Debug.Log("Inventory manager starting...");
        _items = new Dictionary<string, int>();
        Status = ManagerStatus.Started;
    }

    private void DisplayItems()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("Items: ");
        foreach (var pair in _items)
        {
            builder.Append(pair.Key);
            builder.Append("(");
            builder.Append(pair.Value);
            builder.Append(") ");
        }
        Debug.Log(builder.ToString());
    }

    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }
        DisplayItems();
    }

    public List<string> GetItemList()
    {
        return new List<string>(_items.Keys);
    }

    public int GetItemCount(string name)
    {
        if (_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }
}
