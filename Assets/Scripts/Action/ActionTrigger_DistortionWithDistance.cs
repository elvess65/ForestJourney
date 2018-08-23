using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Изменение объекта в зависимости от расстояния до игрока
/// </summary>
public class ActionTrigger_DistortionWithDistance : ActionTrigger, iExitableFromInteractionArea
{
    [Space(10)]
    public MeshRenderer Renderer;

    private Material m_Material;
    private float m_InitDist;
    private float m_InitAlbedo;
    private Utils.InterpolationData<float> m_DistLerpData;
    private Utils.InterpolationData<float> m_AlbedoLerpData;
    private const string m_DIST = "_Distortion";
    private const string m_ALBEDO = "_MainTint";
    private const float m_TARGET_DIST = 0.15f;
    private const float m_TARGET_ALBEDO = 0.4f;

    protected override void Start()
    { 
        base.Start();

        //Создать новый материал
        m_Material = new Material(Renderer.sharedMaterial);
        Renderer.sharedMaterial = m_Material;

        m_DistLerpData = new Utils.InterpolationData<float>();
        m_AlbedoLerpData = new Utils.InterpolationData<float>();
        m_DistLerpData.TotalTime = 1;

        m_InitDist = m_Material.GetFloat(m_DIST);
        m_InitAlbedo = m_Material.GetFloat(m_ALBEDO);
    }

    public override void Interact()
    {
        m_DistLerpData.From = m_Material.GetFloat(m_DIST);
        m_DistLerpData.To = m_TARGET_DIST;

        m_AlbedoLerpData.From = m_Material.GetFloat(m_ALBEDO);
        m_AlbedoLerpData.To = m_TARGET_ALBEDO;


        m_DistLerpData.Start();
    }

    public void ExitFromInteractableArea()
    {
        m_DistLerpData.From = m_Material.GetFloat(m_DIST);
        m_DistLerpData.To = m_InitDist;

        m_AlbedoLerpData.From = m_Material.GetFloat(m_ALBEDO);
        m_AlbedoLerpData.To = m_InitAlbedo;

        m_DistLerpData.Start();
    }

    void Update()
    {
        if (m_DistLerpData.IsStarted)
        {
            m_DistLerpData.Increment();
            float dist = Mathf.Lerp(m_DistLerpData.From, m_DistLerpData.To, m_DistLerpData.Progress);
            m_Material.SetFloat(m_DIST, dist);

            float albedo = Mathf.Lerp(m_AlbedoLerpData.From, m_AlbedoLerpData.To, m_DistLerpData.Progress);
            m_Material.SetFloat(m_ALBEDO, albedo);

            if (m_DistLerpData.Overtime())
            {
                m_DistLerpData.Stop();
            }
        }
    }
}
    
