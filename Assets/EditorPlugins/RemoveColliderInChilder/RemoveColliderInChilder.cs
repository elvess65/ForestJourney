using UnityEngine;

namespace mytest.EditorPlugins.Tools.RemoveColliderInChildren
{
    public class RemoveColliderInChilder : MonoBehaviour
    {
        void RemoveCollider(Transform transform)
        {
            Collider collider = transform.GetComponent<Collider>();
            if (collider != null)
                collider.gameObject.SetActive(false);
        }

        public void DisableColliders()
        {
            foreach (Transform child in transform)
            {
                RemoveCollider(child);

                foreach (Transform grandChild in child)
                    RemoveCollider(grandChild);
            }
        }
    }
}
