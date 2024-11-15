﻿using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Включить какой-то объект
    /// </summary>
    public class TriggerEvent_ActivateObject : TriggerAction_Event
    {
        [Space(10)]
        public GameObject ActivateObject;

        void Awake()
        {
            ActivateObject.SetActive(false);
        }

        protected override void CallEvent()
        {
            ActivateObject.SetActive(true);
        }
    }
}
