using UnityEngine;

public class PlayerCollisionController : CollisionController
{
    private const string m_ACTIONFIELD_RESTART =         "Restart";
    private const string m_ACTIONFIELD_ROTATE_CAMERA =   "RotateCamera";
    private const string m_ACTIONFIELD_REMOVE_OBSTACLE = "RemoveObstacle";
    private const string m_ACTIONFIELD_FINISH_ROUND =    "FinishRound";

    private const string m_OBJECT_ASSISTANT_NAME =       "Assistant";
    private const string m_OBJECT_SOUL_NAME =            "Soul";
    private const string m_OBJECT_ENEMY_NAME =           "Enemy";

    public override void HandleCollistion(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case m_ACTIONFIELD_RESTART:
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                break;
            case m_ACTIONFIELD_ROTATE_CAMERA:
            case m_ACTIONFIELD_REMOVE_OBSTACLE:
                other.GetComponent<Action_Base>().Action();
                break;
            case m_ACTIONFIELD_FINISH_ROUND:
                Debug.Log("FINISH ROUND");
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                break;
            case m_OBJECT_ASSISTANT_NAME:
                AssistantController assistant = other.GetComponent<AssistantController>();
                GameManager.Instance.Player.AddAssistant(assistant);
                assistant.Pick();
                break;
            case m_OBJECT_SOUL_NAME:
                Action_Soul soul = other.GetComponent<Action_Soul>();
                soul.Action();

                GameManager.Instance.Player.AddSoul(soul);
                break;
            case m_OBJECT_ENEMY_NAME:
                enabled = false;
                Instantiate(other.GetComponent<EnemyController>().ExplisionPrefab).transform.position = transform.position;
                Destroy(other.gameObject);
                break;
        }
    }
}
