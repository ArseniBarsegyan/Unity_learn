using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private SceneController _controller;
    [SerializeField] private GameObject _cardBack;

    public int Id { get; private set; }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        if (_cardBack.activeSelf)
        {
            _cardBack.SetActive(false);
        }
    }
}
