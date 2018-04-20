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
    private const string m_PICKABLE =                    "Pickable";

    public override void HandleCollistion(Collider other)
    {
        string tag = other.tag;
        switch (tag)
        {
            case m_ACTIONFIELD_RESTART:
            case m_ACTIONFIELD_ROTATE_CAMERA:
            case m_ACTIONFIELD_REMOVE_OBSTACLE:
            case m_ACTIONFIELD_FINISH_ROUND:
                other.GetComponent<Action_Base>().Interact();
                break;
            case m_OBJECT_ASSISTANT:
                AssistantController assistant = other.GetComponent<AssistantController>();   
                if (GameManager.Instance.Player.AddAssistant(assistant))
                    assistant.Interact();
                break;
            case m_OBJECT_WEAPON:
                Item_Base weapon = other.GetComponent<Item_Base>();
                if (GameManager.Instance.Player.AddWeapon(weapon))
                    weapon.Interact();
                break;
            case m_OBJECT_ENEMY:
                GetComponent<PlayerController>().DestroyPlayer(); 
                other.GetComponent<EnemyController>().TakeDamage(transform);
                break;
            case m_PICKABLE:
                other.GetComponent<Item_Base>().Interact();
                break;
        }
    }
}
