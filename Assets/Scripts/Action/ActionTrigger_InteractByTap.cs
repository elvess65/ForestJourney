using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Объект, который можно активировать по тапу
/// </summary>
public class ActionTrigger_InteractByTap : ActionTrigger, iInteractableByTap
{
    [Header(" - DERRIVED -")]
    public GameObject SelectionEffect;

    private bool m_CanInteract = false;

    public void ExitFromInteractableArea()
    {
        m_CanInteract = false;
        SelectionEffect.gameObject.SetActive(false);
    }

	public void InteractByTap()
	{
        base.Interact();
		SelectionEffect.gameObject.SetActive(false);
	}

	public override void Interact()
	{
		m_CanInteract = true;
        SelectionEffect.gameObject.SetActive(true);
	}

    protected override void Start()
    {
        base.Start();

        SelectionEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        if (m_CanInteract && Input.GetMouseButtonDown(0))
        {
			RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
            {
                iInteractableByTap obj = hit.collider.GetComponentInParent<iInteractableByTap>();
                if (obj != null)
                    obj.InteractByTap();
            }
        }
    }
}
