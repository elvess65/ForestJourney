using mytest.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewBehaviourScript : MonoBehaviour
{
    public Light MainLight;


    InterpolationData<float> m_LerpData;
    InterpolationData<float> m_EnergyLerpData;
    float initIntensity;
    float decreaseTime = 5;
    float increaseTime = 1;

    MazeSpawner m;

	void Start ()
    {
        m = FindObjectOfType<MazeSpawner>();

        initIntensity = MainLight.intensity;

        m_EnergyLerpData = new InterpolationData<float>();
        m_LerpData = new InterpolationData<float>(decreaseTime);
        m_LerpData.From = initIntensity;
        m_LerpData.To = 0;

        m_EnergyLerpData.From = NewBehaviourScript1.Instance.Energy;
        m_EnergyLerpData.To = 0;

        m_LerpData.Start();
	}

    public List<MazeCell> visitedCells = new List<MazeCell>();

    private void FixedUpdate()
    {
        MazeCell currentCell = m.GetCellByPosition(transform.position);
        if (!visitedCells.Contains(currentCell))
        {
            visitedCells.Add(currentCell);
            Debug.Log(currentCell.ToString());
        }
    }

    private void OnDestroy()
    {
        string str = string.Empty;
        for (int i = 0; i < visitedCells.Count - 1; i++)
            str += visitedCells[i].Column + "." + visitedCells[i].Row + "|";

        str += visitedCells[visitedCells.Count - 1].Column + "." + visitedCells[visitedCells.Count - 1].Row;

        PlayerPrefs.SetString("str", str);
    }

    bool increasing = false;
	void Update ()
    {       
        if (Input.GetKeyDown(KeyCode.L))
        {
            NewBehaviourScript1.Instance.UseStamina();

            increasing = true;
            m_LerpData.TotalTime = increaseTime;
            m_LerpData.From = MainLight.intensity;
            m_LerpData.To = initIntensity;

            m_EnergyLerpData.From = NewBehaviourScript1.Instance.CurEnergy;
            m_EnergyLerpData.To = NewBehaviourScript1.Instance.Energy;


            m_LerpData.Start();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //Instantiate(ob, transform.position, Quaternion.identity);
        }

        if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment();
            MainLight.intensity = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
            NewBehaviourScript1.Instance.UpdateEnergy(Mathf.Lerp(m_EnergyLerpData.From, m_EnergyLerpData.To, m_LerpData.Progress));

            if (m_LerpData.Overtime())
            {
                m_LerpData.Stop();

                if (increasing)
                {
                    increasing = false;
                    m_LerpData.TotalTime = decreaseTime;
					m_LerpData.From = initIntensity;
					m_LerpData.To = 0;

					m_EnergyLerpData.From = NewBehaviourScript1.Instance.Energy;
					m_EnergyLerpData.To = 0;

					m_LerpData.Start();
                }
            }
        }
	}
}
