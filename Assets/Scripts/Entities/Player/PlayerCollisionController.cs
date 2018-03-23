using UnityEngine;

public class PlayerCollisionController : CollisionController
{
    private const string m_ACTIONFIELD_RESTART =         "Restart";
    private const string m_ACTIONFIELD_ROTATE_CAMERA =   "RotateCamera";
    private const string m_ACTIONFIELD_REMOVE_OBSTACLE = "RemoveObstacle";
    private const string m_ACTIONFIELD_FINISH_ROUND =    "FinishRound";

    private const string m_OBJECT_ASSISTANT =            "Assistant";
    private const string m_OBJECT_WEAPON =               "Weapon";
    private const string m_OBJECT_ENEMY =                "Enemy";

    public override void HandleCollistion(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case m_ACTIONFIELD_RESTART:
            case m_ACTIONFIELD_ROTATE_CAMERA:
            case m_ACTIONFIELD_REMOVE_OBSTACLE:
            case m_ACTIONFIELD_FINISH_ROUND:
                other.GetComponent<Action_Base>().Action();
                break;
            case m_OBJECT_ASSISTANT:
                AssistantController assistant = other.GetComponent<AssistantController>();   
                assistant.Pick();

                GameManager.Instance.Player.AddAssistant(assistant);
                break;
            case m_OBJECT_WEAPON:
                Pickable_Base weapon = other.GetComponent<Pickable_Base>();
                weapon.Pick();

                GameManager.Instance.Player.AddWeapon(weapon);
                break;
            case m_OBJECT_ENEMY:
                GetComponent<PlayerController>().DestroyPlayer(); 
                other.GetComponent<EnemyController>().TakeDamage(transform);
                break;
        }
    }
}
