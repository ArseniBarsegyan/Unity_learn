using System.Collections.Generic;
using UnityEngine;

public class PlayerCompass : MonoBehaviour
{
    Board m_board;

    public GameObject arrowPrefab;
    List<GameObject> m_arrows = new List<GameObject>();

    public float scale = 1f;
    public float startDistance = 0.25f;
    public float endDistance = 0.5f;

    public float moveTime = 1f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public float delay = 0f;

    void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        SetupArrows();
        MoveArrows();
    }

    void SetupArrows()
    {
        if (arrowPrefab == null)
        {
            Debug.LogWarning("Missing arrow prefab");
            return;
        }

        foreach (Vector2 dir in Board.directions)
        {
            Vector3 dirVector = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
            Quaternion rotation = Quaternion.LookRotation(dirVector);
            GameObject arrowInstance = Instantiate(
                arrowPrefab, transform.position + dirVector * startDistance, rotation);            
            arrowInstance.transform.localScale = new Vector3(scale, scale, scale);
            arrowInstance.transform.parent = transform;
            m_arrows.Add(arrowInstance);
        }
    }

    void MoveArrow(GameObject arrowInstance)
    {
        iTween.MoveBy(gameObject, iTween.Hash(
            "z", endDistance - startDistance,
            "looptype", iTween.LoopType.loop,
            "time", moveTime,
            "easeType", easeType
            ));
    }

    void MoveArrows()
    {
        foreach (var arrow in m_arrows)
        {
            MoveArrow(arrow);
        }
    }
}
