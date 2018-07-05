using UnityEngine;

public class Object_RepairBehaviour_Item : MonoBehaviour
{
    public TransformData RepairedTransfromData;
    public TransformData DestroyedTransformData;
    public float delay = 0;

    private TransformData m_From;
    private TransformData m_To;
    private System.Action OnAnimationFinished;

    private float m_CurTime;
    private float m_TotalTime = 0.5f;
    private bool m_IsAnimating = false;

    public void SetRepairedImmediate()
    {
        transform.localPosition = RepairedTransfromData.Position;
        transform.localRotation = RepairedTransfromData.Rotation;
    }

    public void SetDestroyedImmediate()
    {
        transform.localPosition = DestroyedTransformData.Position;
        transform.localRotation = DestroyedTransformData.Rotation;
    }

    public void SaveTransformAsRepaired()
    {
        RepairedTransfromData = GetTransform();
    }

    public void SaveTransfromAsDestroyed()
    {
        DestroyedTransformData = GetTransform();
    }

    public void PrepareToRepairedAnimation()
    {
        OnAnimationFinished = SetRepairedImmediate;
        m_From = GetTransform();
        m_To = RepairedTransfromData;
        m_CurTime = 0;
        m_IsAnimating = true;
    }

    public void PrepareToDestroyedAnimation()
    {
        OnAnimationFinished = SetDestroyedImmediate;
        m_From = GetTransform();
        m_To = DestroyedTransformData;
        m_CurTime = 0;
        m_IsAnimating = true;
    }

    public void AnimationFinished()
    {
        if (OnAnimationFinished != null)
            OnAnimationFinished();
    }

    public void UpdateTransform(float progress)
    {
        transform.localPosition = Vector3.Slerp(m_From.Position, m_To.Position, progress);
        transform.localRotation = Quaternion.Slerp(m_From.Rotation, m_To.Rotation, progress);   
    }

    public float progress = 0;
    private void Update()
    {
        if (m_IsAnimating)
        {
            m_CurTime += Time.deltaTime;
            progress = m_CurTime / m_TotalTime;
            UpdateTransform(progress);
            
            if (m_CurTime >= m_TotalTime)
            {
                m_IsAnimating = false;
                AnimationFinished();
            }
        }
    }


    TransformData GetTransform()
    {
        return new TransformData(transform.localPosition, transform.localRotation);
    }

    [System.Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public TransformData(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }
}
