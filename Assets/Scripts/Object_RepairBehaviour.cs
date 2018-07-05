using System.Collections.Generic;
using UnityEngine;

public class Object_RepairBehaviour : MonoBehaviour
{
    public bool RepairedIsDefault = true;
    public List<Object_RepairBehaviour_Item> ObjectItems;

    private float m_CurTime;
    private float m_TotalTime = 2;
    private bool m_IsAnimating = false;

    private void Start()
    {
        if (!RepairedIsDefault)
            SetDestroyedImmediate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            AnimateToDestroyed();

        if (Input.GetKeyDown(KeyCode.R))
            AnimateToRepaired();

        /*if (m_IsAnimating)
        {
            m_CurTime += Time.deltaTime;

            for (int i = 0; i < ObjectItems.Count; i++)
                ObjectItems[i].Update(m_CurTime / m_TotalTime);

            if (m_CurTime >= m_TotalTime)
            {
                m_IsAnimating = false;

                for (int i = 0; i < ObjectItems.Count; i++)
                    ObjectItems[i].AnimationFinished();
            }
        }*/

        if (m_IsAnimating)
        {
            if (ObjectItems[curIndex].progress >= 0.5f)
                ObjectItems[++curIndex].PrepareToRepairedAnimation();
        }
    }

    int curIndex = 0;
    public void AnimateToRepaired()
    {
        ObjectItems[curIndex].PrepareToRepairedAnimation();

        m_CurTime = 0;
        m_IsAnimating = true;
    }

    public void AnimateToDestroyed()
    {
        for (int i = 0; i < ObjectItems.Count; i++)
            ObjectItems[i].PrepareToDestroyedAnimation();

        m_CurTime = 0;
        m_IsAnimating = true;
    }

    public void SetRepairedImmediate()
    {
        for (int i = 0; i < ObjectItems.Count; i++)
            ObjectItems[i].SetRepairedImmediate();
    }

    public void SetDestroyedImmediate()
    {
        for (int i = 0; i < ObjectItems.Count; i++)
            ObjectItems[i].SetDestroyedImmediate();
    }
}
