using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public System.Action<bool> OnInputStateChange;

    public KeyboardInputManager KeyboardInput;
    public VirtualJoystickInputManager VirtualJoystickInput;

    private bool m_InputState = false;

    public bool InputIsEnabled
    {
        get { return m_InputState; }
        set 
        {
            m_InputState = value;

            if (OnInputStateChange != null)
                OnInputStateChange(m_InputState);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.U))
            InputIsEnabled = true;

        if (Input.GetKeyDown(KeyCode.L))
            InputIsEnabled = false;

        if (GameManager.Instance.IsActive && m_InputState)
        {
            KeyboardInput.UpdateInput();
            VirtualJoystickInput.UpdateInput();
        }
    }

    public void UnlockInput()
    {
        InputIsEnabled = true;
    }
}

public abstract class BaseInputManager : MonoBehaviour
{
    public System.Action<Vector3> OnMove;

    public bool Enabled = true;

    public abstract void UpdateInput();
}
