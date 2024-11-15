﻿using mytest.Effects.Custom.Compass;
using System.Collections.Generic;
using UnityEngine;

namespace mytest.Main
{
    public class GameStateController : MonoBehaviour
    {
        public CompassWorldBehaviour GlobalCompass;

        private PlayerController m_Player;
        private GameOverController m_GameOverController;
        private List<EnemyController> m_Enemies;

        public enum KeyTypes
        {
            Key1,
            Key2,
            Key3
        }
        private Dictionary<KeyTypes, int> m_CollectedKeys;

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

        private void Start()
        {
            m_GameOverController = GetComponent<GameOverController>();
        }

        public void AddEnemy(EnemyController enemy)
        {
            if (m_Enemies == null)
                m_Enemies = new List<EnemyController>();

            m_Enemies.Add(enemy);
        }

        public void AddKey(KeyTypes type)
        {
            if (m_CollectedKeys == null)
                m_CollectedKeys = new Dictionary<KeyTypes, int>();

            if (!m_CollectedKeys.ContainsKey(type))
                m_CollectedKeys.Add(type, 0);

            m_CollectedKeys[type]++;
        }

        public void RemoveKey(KeyTypes type)
        {
            if (m_CollectedKeys == null)
                return;

            if (m_CollectedKeys.ContainsKey(type))
                m_CollectedKeys[type]--;

            if (m_CollectedKeys[type] <= 0)
                m_CollectedKeys.Remove(type);
        }

        public bool HasKeysForActivation(KeyTypes[] keys)
        {
            bool result = false;
            if (m_CollectedKeys != null)
            {
                for (int i = 0; i < keys.Length; i++)
                    result = m_CollectedKeys.ContainsKey(keys[i]);
            }

            return result;
        }

        public void GameOver()
        {
            m_GameOverController.GameOver();
        }
    }
}
