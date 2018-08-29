using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript1 : MonoBehaviour
{

    public static NewBehaviourScript1 Instance;

    public Image StaminaImage;
    public Image EnergyImage;

    public int Stamina = 10;
    public int Energy = 20;
    public int StaminaForUse = 1;

    public int m_CurStamina;
    public float CurEnergy;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
		m_CurStamina = Stamina;
        CurEnergy = Energy;
    }

    public void UseStamina()
    {
        m_CurStamina -= StaminaForUse;

        if (m_CurStamina <= 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        
        float progress = (float)m_CurStamina / Stamina;
        StaminaImage.fillAmount = progress;
    }

    public void UpdateEnergy(float curEnergy)
    {
        CurEnergy = curEnergy;

        float progress = CurEnergy / Energy;

        EnergyImage.fillAmount = progress;
    }
}
