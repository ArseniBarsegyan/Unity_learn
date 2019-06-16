using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private GameObject _enemy;

    void Update()
    {
        if (_enemy == null)
        {
            _enemy = Instantiate(enemyPrefab) as GameObject;
            var aiComponent = _enemy.GetComponent<WanderingAI>();
            if (PlayerPrefs.GetFloat("speed") == 0)
            {
                aiComponent.speed = aiComponent.baseSpeed;
            }
            else
            {
                aiComponent.speed = aiComponent.baseSpeed * PlayerPrefs.GetFloat("speed");
            }
            _enemy.transform.position = new Vector3(0, 1, 0);
            float angle = Random.Range(0, 360);
            _enemy.transform.Rotate(0, angle, 0);
        }
    }
}
