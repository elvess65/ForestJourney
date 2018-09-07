﻿using mytest.Main.MiniGames;
using UnityEngine;

namespace mytest.UI.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public MiniGameController MiniGame;
        public MiniGame_Base Game;

        public static InputManager Instance;
        public System.Action<bool> OnInputStateChange;

        public KeyboardInputManager KeyboardInput;
        public VirtualJoystickInputManager VirtualJoystickInput;
        public bool PreferVirtualJoystickInEditor = false;

        private bool m_InputState = false;
        private BaseInputManager m_Input;

        public bool InputIsEnabled
        {
            get { return m_InputState; }
            set
            {
                if (m_InputState != value)
                {
                    m_InputState = value;

                    if (OnInputStateChange != null)
                        OnInputStateChange(m_InputState);
                }
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
#if UNITY_EDITOR
            if (PreferVirtualJoystickInEditor)
                m_Input = VirtualJoystickInput;
            else
                m_Input = KeyboardInput;
#else
        m_Input = VirtualJoystickInput;
#endif
        }

        void Update()
        {
            if (GameManager.Instance.IsActive && m_InputState)
                m_Input.UpdateInput();

            if (Input.GetKeyDown(KeyCode.L))
                InputIsEnabled = false;

            if (Input.GetKeyDown(KeyCode.U))
                InputIsEnabled = true;

            if (Input.GetKeyDown(KeyCode.G))
            {
                InputIsEnabled = false;
                MiniGame.OnGameStarted += Started;
                MiniGame.OnGameFinished += Finished;
                MiniGame.StartGame(Game);
            }
        }

        void Started()
        {
            Debug.Log("Game started");
        }

        void Finished()
        {
            Debug.Log("Game finished");

            InputIsEnabled = true;
        }

        public void UnlockInput()
        {
            InputIsEnabled = true;
        }
    }

    public abstract class BaseInputManager : MonoBehaviour
    {
        public System.Action<Vector3> OnMove;

        public abstract void UpdateInput();
    }
}
