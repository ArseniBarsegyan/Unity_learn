using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private SceneController controller;
    [SerializeField] private GameObject cardBack;

    public int Id { get; private set; }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf && controller.canReveal)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
