using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;
    private Dictionary<string, int> _items;

    public ManagerStatus Status { get; private set; }
    public string EquippedItem { get; private set; }

    public bool EquipItem(string name)
    {
        if (_items.ContainsKey(name) && EquippedItem != name)
        {
            EquippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }
        EquippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory manager starting...");
        _network = service;
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

    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            if (_items[name] == 0)
            {
                _items.Remove(name);
            }
        }
        else
        {
            Debug.Log("cannot consume " + name);
            return false;
        }
        DisplayItems();
        return true;
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
