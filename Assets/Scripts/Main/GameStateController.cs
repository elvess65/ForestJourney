using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    private PlayerController m_Player;
    private List<EnemyController> m_Enemies;

    private Dictionary<KeyController.KeyTypes, int> m_CollectedKeys;

    public PlayerController Player
    {
        get { return m_Player; }
        set { m_Player = value; }
    }
    public List<EnemyController> Enemies
    {
        get
        {
            if (m_Enemies == null)
                m_Enemies = new List<EnemyController>();

            return m_Enemies;
        }
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (m_Enemies == null)
            m_Enemies = new List<EnemyController>();

        m_Enemies.Add(enemy);
    }

    public void AddKey(KeyController.KeyTypes type)
    {
        if (m_CollectedKeys == null)
            m_CollectedKeys = new Dictionary<KeyController.KeyTypes, int>();

        if (!m_CollectedKeys.ContainsKey(type))
            m_CollectedKeys.Add(type, 0);

        m_CollectedKeys[type]++;
    }

    public void RemoveKey(KeyController.KeyTypes type)
    {
        if (m_CollectedKeys == null)
            return;

        if (m_CollectedKeys.ContainsKey(type))
            m_CollectedKeys[type]--;

        if (m_CollectedKeys[type] <= 0)
            m_CollectedKeys.Remove(type);
    }

    public bool HasKeysForActivation(KeyController.KeyTypes[] keys)
    {
        bool result = false;
        if (m_CollectedKeys != null)
        {
            for (int i = 0; i < keys.Length; i++)
                result = m_CollectedKeys.ContainsKey(keys[i]);
        }

        return result;
    }
}
