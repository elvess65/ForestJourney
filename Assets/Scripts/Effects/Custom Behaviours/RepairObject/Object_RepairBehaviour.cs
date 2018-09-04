using System.Collections.Generic;
using UnityEngine;

public class Object_RepairBehaviour : MonoBehaviour
{
    public System.Action OnAnimationFinished;

    public bool RepairedIsDefault = true;
    public float ItemAnimationTime = 0.5f;
    public List<Object_RepairBehaviour_Item> ObjectItems;

    private int m_FinishedGroups = 0;
	private Dictionary<int, ItemGroup> m_ItemGroups;

    void Start()
    {
        if (!RepairedIsDefault)
            SetDestroyedImmediate();

        OnAnimationFinished += AnimationFinishedHandler;

        m_ItemGroups = new Dictionary<int, ItemGroup> ();
		for (int i = 0; i < ObjectItems.Count; i++) 
		{
			int groupID = ObjectItems [i].GroupID;
			if (!m_ItemGroups.ContainsKey (groupID)) 
				m_ItemGroups.Add (groupID, new ItemGroup());

			m_ItemGroups [groupID].AddIndex (i);

            ObjectItems[i].AnimationTime = ItemAnimationTime;
        }
    }

    void AnimationFinishedHandler()
    {
        foreach (ItemGroup group in m_ItemGroups.Values)
            group.ResetIndex();
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

    public void Animate()
    {
        foreach (int groupID in m_ItemGroups.Keys)
            AnimateNext(groupID);
    }

	void AnimateNext(int groupID)
	{
		int index = m_ItemGroups [groupID].GetNextIndex ();
		if (index >= 0) 
		{
			ObjectItems [index].OnAllowAnimateNext += AnimateNext;
			ObjectItems [index].Animate ();
		} 
		else
        {
            if (++m_FinishedGroups >= m_ItemGroups.Count)
            {
                if (OnAnimationFinished != null)
                    OnAnimationFinished();
            }
        }
	}


	[System.Serializable]
	class ItemGroup
	{
		public int m_Index;
		private List<int> m_ItemIndexes;

		public ItemGroup()
		{
			m_ItemIndexes = new List<int>();

            ResetIndex();
		}

		public void AddIndex(int index)
		{
			m_ItemIndexes.Add (index);
		}

		public int GetNextIndex()
		{
			if (m_Index >= m_ItemIndexes.Count - 1)
				return -1;

			return m_ItemIndexes [++m_Index];
		}

        public void ResetIndex()
        {
            m_Index = -1;
        }
	}
}
