using UnityEngine;

public class PlayerCollisionController : CollisionController
{
    private const string m_ACTIONFIELD_INTERACTABLE =    "Intaractable";
    private const string m_OBJECT_ENEMY =                "Enemy";

    public override void HandleEnterCollision(Collider other)
    {
        string objTag = other.tag;
        switch (objTag)
        {
            case m_ACTIONFIELD_INTERACTABLE:
                other.GetComponent<iInteractable>().Interact();
                break;
            case m_OBJECT_ENEMY:
                GetComponent<PlayerController>().DestroyPlayer(); 
                other.GetComponent<EnemyController>().TakeDamage(transform);
                break;
        }
    }

    public override void HandlerExitCollision(Collider other)
    {
		string objTag = other.tag;
        switch (objTag)
        {
            case m_ACTIONFIELD_INTERACTABLE:
                other.GetComponent<iInteractableByTap>().ExitFromInteractableArea();
                break;
        }
    }
}
