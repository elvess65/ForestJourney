﻿using UnityEngine;

[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float MoveSpeed = 3;
    public float RotateSpeed = 5;

    private Vector3 m_MoveDir = Vector3.zero;
    private CollisionController m_CollisionController;
    private CharacterController m_CharacterController;
    private PlayerAnimationController m_PlayerAnimationController;

    private Item_Base m_Assistant;
    private WeaponController m_Weapon;
    private Quaternion m_TargetRot;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CollisionController = GetComponent<PlayerCollisionController>();
        m_PlayerAnimationController = GetComponent<PlayerAnimationController>();

        m_TargetRot = transform.rotation;

        InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove += MoveInDir;
        InputManager.Instance.OnInputStateChange += InputStatusChangeHandler;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRot, Time.deltaTime * RotateSpeed);
    }

    public void MoveInDir(Vector3 dir)
    {
        m_MoveDir = dir;

        if (m_MoveDir != Vector3.zero)
        {
            //Rotate in move dir
            float angle = Mathf.Atan2(m_MoveDir.x, m_MoveDir.z) * Mathf.Rad2Deg;
            m_TargetRot = Quaternion.AngleAxis(angle, Vector3.up);

			//Move
            m_CharacterController.SimpleMove(m_MoveDir * MoveSpeed);
        }

        //Move animation
        m_PlayerAnimationController.PlayMoveAnimation(m_MoveDir.magnitude);
    }

    public void DestroyPlayer()
    {
        InputManager.Instance.VirtualJoystickInput.OnMove -= MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove -= MoveInDir;

        m_PlayerAnimationController.PlayMoveAnimation(0);
        m_PlayerAnimationController.enabled = false;
        m_CollisionController.enabled = false;
        enabled = false;
    }

	private void InputStatusChangeHandler(bool state)
	{
        if (!state)
            m_PlayerAnimationController.StopPlayerMoveAnimation();
	}


	public bool TryAddAssistant(Item_Base assistant)
    {
        if (m_Assistant != null)
            return false;

        m_Assistant = assistant;

        return true;
    }

    public bool TryAddWeapon(Item_Base weapon)
    {
        if (m_Weapon != null)
            return false;

        m_Weapon = (WeaponController)weapon;

        return true;
    }

    public void UseAssistant()
    {
        if (m_Assistant != null)
        {
            m_Assistant.Use();
            m_Assistant = null;
        }
    }

    public void UseWeapon()
    {
        if (m_Weapon != null)
        {
            m_Weapon.Use();
            m_Weapon = null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        m_CollisionController.HandleCollistion(other); 
    }
}
