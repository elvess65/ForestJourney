using System.Collections.Generic;
using UnityEngine;

public class Object_RepairBehaviour : MonoBehaviour
{
    public bool RepairedIsDefault = true;
    public List<Object_RepairBehaviour_Item> ObjectItems;

    private float m_CurTime;
    private float m_TotalTime = 2;
    private bool m_IsAnimating = false;
	private Dictionary<int, ItemGroup> m_ItemGroups;

    private void Start()
    {
        if (!RepairedIsDefault)
            SetDestroyedImmediate();

		m_ItemGroups = new Dictionary<int, ItemGroup> ();
		for (int i = 0; i < ObjectItems.Count; i++) 
		{
			int groupID = ObjectItems [i].GroupID;
			if (!m_ItemGroups.ContainsKey (groupID)) 
				m_ItemGroups.Add (groupID, new ItemGroup());

			m_ItemGroups [groupID].AddIndex (i);
		}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            AnimateToRepaired();
    }
		
    public void AnimateToRepaired()
    {
		foreach (int groupID in m_ItemGroups.Keys) 
			AnimateNextToRepaired (groupID);
    }

	void AnimateNextToRepaired(int groupID)
	{
		int index = m_ItemGroups [groupID].GetNextIndex ();
		if (index >= 0) 
		{
			ObjectItems [index].OnAllowAnimateNext += AnimateNextToRepaired;
			ObjectItems [index].AnimateToRepaired ();
		} 
		else
			Debug.Log ("Animation finished");
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


	[System.Serializable]
	class ItemGroup
	{
		public int m_Index;
		private List<int> m_ItemIndexes;

		public ItemGroup()
		{
			m_ItemIndexes = new List<int>();

			m_Index = -1;
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
	}
}
