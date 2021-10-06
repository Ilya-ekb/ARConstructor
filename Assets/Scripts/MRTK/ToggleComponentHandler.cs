using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleComponentHandler : MonoBehaviour 
{
    [SerializeField] private MonoBehaviour component;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDisableEvent;

    private void OnEnable()
    {
        if (component.enabled)
        {
            OnEnableEvent?.Invoke();
        }
        else
        {
            OnDisableEvent?.Invoke();
        }
    }

    public void OnToggleComponent()
    {
        if (component == null)
        {
            return;
        }

        component.enabled = !component.enabled;
        if (component.enabled)
        {
            OnEnableEvent?.Invoke();
            return;
        }
        OnDisableEvent?.Invoke();
    }
}
