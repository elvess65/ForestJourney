using mytest.Effects;
using mytest.Interaction;
using UnityEngine;
using mytest.Effects.Custom;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Класс тумблера
    /// Объект, который можно активировать по тапу.
    /// Отслеживает вход/выход зоны взаимодествия
    /// При тапе ведет себе аналогично ActionTrigger. 
    /// </summary>
    public class ActionToggle : ActionToggle_Base
    {
        [Space(10)]
        [Header("Effects")]
        [Tooltip("Эффект, который активен в обычном состоянии")]
        public Effect_Base IdleEffect;
        [Tooltip("Список эффектов, которые активируються при выделении")]
        public Effect_Base[] SelectionEffects;
        [Header("Emissions")]
        [Tooltip("Список контроллеров эмиссии (на самом объекте и руны")]
        public EmissionEffectBehaviour[] EmissionBehaviours;

        public override void ExitFromInteractableArea()
        {
            base.ExitFromInteractableArea();

			//Изменить состояние эммиссии
			for (int i = 0; i < EmissionBehaviours.Length; i++)
				EmissionBehaviours[i].SetIdle();

            //Включить пасивный эффект
            IdleEffect.Activate();
        }

        public override void InteractByTap()
        {
            base.InteractByTap();


			//Изменить состояние эммиссии
			for (int i = 0; i < EmissionBehaviours.Length; i++)
				EmissionBehaviours[i].SetDisable();
        }

        public override void Interact()
        {
            base.Interact();

            //Выключить пасивный эффект
            IdleEffect.Deactivate();

            //Включить активный эффект 
            for (int i = 0; i < SelectionEffects.Length; i++)
                SelectionEffects[i].Activate();

            //Изменить состояние эммиссии
            for (int i = 0; i < EmissionBehaviours.Length; i++)
                EmissionBehaviours[i].SetMax();
        }


        protected override void Unselect()
        {
            base.Unselect();

            //Выключить активный эффект
            for (int i = 0; i < SelectionEffects.Length; i++)
                SelectionEffects[i].Deactivate();
        }

        protected override void Start()
        {
            base.Start();

            //Включить пасивный эффект
            IdleEffect.Activate();
        }
    }
}
