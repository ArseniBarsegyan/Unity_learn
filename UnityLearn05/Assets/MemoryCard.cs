using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject _cardBack;
    [SerializeField] private Sprite _image;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _image;
    }

    public void OnMouseDown()
    {
        if (_cardBack.activeSelf)
        {
            _cardBack.SetActive(false);
        }
    }
}
