using System.Collections.Generic;
using UnityEngine;

public class UIWindowsTutorialLibrary : MonoBehaviour
{
    public static UIWindowsTutorialLibrary Instance;
    public TutorialWindowListEntry[] TutorialWindowsList;

	public enum TutorialWindowTypes
	{
		Invitation,
		FinishRound,
        FinishRoundObstacle,
        Assistant,
        DisableObstacle,
        Light,
        CameraRotation
	}

    private Dictionary<TutorialWindowTypes, UIWindow_Base> m_Windows;

    private void Awake()
    {
        Instance = this;
        m_Windows = new Dictionary<TutorialWindowTypes, UIWindow_Base>();

        for (int i = 0; i < TutorialWindowsList.Length; i++)
        {
            if (!m_Windows.ContainsKey(TutorialWindowsList[i].Type))
                m_Windows.Add(TutorialWindowsList[i].Type, TutorialWindowsList[i].WindowPrefab);
        }
    }

    public UIWindow_Base GetWindowPrefabByType(TutorialWindowTypes type)
    {
        if (m_Windows.ContainsKey(type))
            return m_Windows[type];

        return null;
    }

    [System.Serializable]
    public class TutorialWindowListEntry
    {
        public TutorialWindowTypes Type;
        public UIWindow_Base WindowPrefab;
    }
}
