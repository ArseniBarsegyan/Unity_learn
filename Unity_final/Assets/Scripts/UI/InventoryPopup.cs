using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Text[] itemLabels;

    [SerializeField] private Text currentItemLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;

    private string _currentItem;

    public void Refresh()
    {
        var itemsList = Managers.Inventory.GetItemList();

        int length = itemIcons.Length;
        for (int i = 0; i < length; i++)
        {
            if (i < itemsList.Count)
            {
                itemIcons.ElementAt(i).gameObject.SetActive(true);
                itemLabels.ElementAt(i).gameObject.SetActive(true);

                string item = itemsList[i];

                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize();

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;

                if (item == Managers.Inventory.EquippedItem)
                {
                    message = "Equipped\n" + message;
                }
                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) => {
                    OnItem(item);
                });
                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemsList.Contains(_currentItem))
        {
            _currentItem = null;
        }
        if (_currentItem == null)
        {
            currentItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            currentItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (_currentItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }
            currentItemLabel.text = _currentItem + ":";
        }
    }

    public void OnItem(string item)
    {
        _currentItem = item;
        Refresh();
    }

    public void OnEquip()
    {
        Managers.Inventory.EquipItem(_currentItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(_currentItem);
        if (_currentItem == "health")
        {
            Managers.Player.ChangeHealth(25);
        }
        Refresh();
    }
}
