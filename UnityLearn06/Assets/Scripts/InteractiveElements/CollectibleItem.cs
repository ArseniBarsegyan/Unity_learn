using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;

    void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(name);
        Destroy(this.gameObject);
    }
}
