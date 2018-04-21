using UnityEngine;

public abstract class CollisionController : MonoBehaviour
{
    public abstract void HandleCollistion(Collider other);
}
