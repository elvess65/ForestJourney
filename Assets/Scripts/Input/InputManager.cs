using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public KeyboardInputManager KeyboardInput;
    public VirtualJoystickInputManager VirtualJoystickInput;

    private void Awake()
    {
        Instance = this;
    }

	void Update ()
    {
        KeyboardInput.Update();
        VirtualJoystickInput.Update();
    }
}

public abstract class BaseInputManager : MonoBehaviour
{
    public System.Action<Vector3> OnMove;

    public bool Enabled = true;

    public abstract void Update();
}
