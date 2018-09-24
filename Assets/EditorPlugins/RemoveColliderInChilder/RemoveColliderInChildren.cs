using UnityEngine;

namespace mytest.EditorPlugins.Tools.RemoveColliderInChildren
{
    public class RemoveColliderInChildren : MonoBehaviour
    {
        public bool DisableGameObject = true;

        void RemoveCollider(Collider collider)
        {
            if (collider != null)
            {
                if (DisableGameObject)
                    collider.gameObject.SetActive(false);
                else
                    collider.enabled = false;
            }
        }

        public void DisableColliders()
        {
            Collider[] allChildren = GetComponentsInChildren<Collider>();
            foreach (Collider collider in allChildren)
                RemoveCollider(collider);

            DestroyImmediate(this);
        }
    }
}
