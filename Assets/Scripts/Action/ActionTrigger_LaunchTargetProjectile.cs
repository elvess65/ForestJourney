using mytest.Interaction;
using System.Collections;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Запуск объектов по цели (выстрел по земле)
    /// </summary>
    public class ActionTrigger_LaunchTargetProjectile : ActionTrigger, iExitableFromInteractionArea
    {
        public Object_RepairBehaviour Behaviour;

        [Space(10)]
        [Tooltip("Цыклический запуск снарядов пока игрок находиться в области")]
        public bool CycleLaunchWhileInArea = false;
        [Tooltip("После первого приминения взаимодейтсовать с объектом будет нельзя")]
        public bool OneTimeLaunch = true;
        public float DelayBeforeFirstLaunch = 0f;
        public float DelayBetweenCycles = 2;
        public TargetDelayedProjectile[] ProjectileLaunchers;
        public float[] DelaysBetweenProjectiles;

        private bool m_IsInArea = false;
        private WaitForSeconds m_CycleWaitDelay = null;
        private WaitForSeconds m_FirstWaitDelay = null;
        private Coroutine m_LaunchSequence = null;
        private Coroutine m_FirstLaunchSequence = null;

        public override void Interact()
        {
            m_IsInArea = true;

            if (m_FirstWaitDelay == null && DelayBeforeFirstLaunch > 0)
                m_FirstWaitDelay = new WaitForSeconds(DelayBeforeFirstLaunch);

            if (CycleLaunchWhileInArea && m_CycleWaitDelay == null)
                m_CycleWaitDelay = new WaitForSeconds(DelayBetweenCycles);

            if (DelayBeforeFirstLaunch > 0)
            {
                if (m_FirstLaunchSequence != null)
                    StopCoroutine(m_FirstLaunchSequence);

                m_FirstLaunchSequence = StartCoroutine(FirstLaunchDelay());
            }
            else
                Launch();
        }

        public void ExitFromInteractableArea()
        {
            m_IsInArea = false;

            if (OneTimeLaunch)
                Deactivate();
        }


        void Launch()
        {
            if (m_LaunchSequence == null)
                m_LaunchSequence = StartCoroutine(LauchWithDelay());
        }

        IEnumerator LauchWithDelay()
        {
            for (int i = 0; i < ProjectileLaunchers.Length; i++)
            {
                ProjectileLaunchers[i].LaunchProjectile();

                yield return new WaitForSeconds(DelaysBetweenProjectiles[i]);
            }

            m_LaunchSequence = null;

            //Цыклический запуск
            if (CycleLaunchWhileInArea && m_IsInArea)
            {
                yield return m_CycleWaitDelay;

                //За время между цыклами игрок может выйти из зоны
                if (m_IsInArea)
                    Launch();
            }
        }

        IEnumerator FirstLaunchDelay()
        {
            yield return m_FirstWaitDelay;

            m_FirstLaunchSequence = null;
            Launch();
        }
    }
}