using System.Collections;
using System.Collections.Generic;
using LogicScripts;
using UnityEngine;
using UnityEngine.Events;

namespace HologramObject
{
    public class TouchEvent : MonoBehaviour
    {
        public UnityEvent OnTouchEvent;

        private void OnTriggerEnter(Collider other)
        {
            OnTouchEvent?.Invoke();
        }

        public void InvokeScenarioMethod(string methodName)
        {
            if (Logic.Instance.Scenario == null || string.IsNullOrEmpty(methodName))
            {
                return;
            }
            Logic.Instance.InvokeFunction(methodName, new object[] { });
        }
    }
}
