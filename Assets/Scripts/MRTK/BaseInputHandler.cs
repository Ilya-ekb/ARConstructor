using System;
using MagicLeap.MRTK.DeviceManagement.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

namespace MRTK
{
    public class BaseInputHandler : MonoBehaviour, IMixedRealitySourceStateHandler, IMixedRealityInputHandler, IMixedRealityInputHandler<Vector2>
    {
        public Action<InputEventData> ADownAction;
        public Action<InputEventData> AUpAction;
        public Action<InputEventData<Vector2>> APositionAction;

        public Action<InputEventData> BDownAction;
        public Action<InputEventData> BUpAction;
        public Action<InputEventData<Vector2>> BPositionAction;

        public Action<InputEventData> CDownAction;
        public Action<InputEventData> CUpAction;
        public Action<InputEventData<Vector2>> CPositionAction;

        public Action<InputEventData> ABDownAction;
        public Action<InputEventData> ACDownAction;
        public Action<InputEventData> BCDownAction;

        [SerializeField] private MixedRealityInputAction AButtonAction;
        [SerializeField] private MixedRealityInputAction BButtonAction;
        [SerializeField] private MixedRealityInputAction CButtonAction;
        [SerializeField] private GameObject extraObject;

        private bool aPress;
        private bool bPress;
        private bool cPress;

        private void OnEnable()
        {
            ResetAllActions();
            if (extraObject != null)
            {
                extraObject.SetActive(true);
            }
            CoreServices.InputSystem?.RegisterHandler<IMixedRealitySourceStateHandler>(this);
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        }

        private void OnDisable()
        {
            ResetAllActions();
            if (extraObject != null)
            {
                extraObject.SetActive(false);
            }
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealitySourceStateHandler>(this);
            CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        }

        public void OnSourceDetected(SourceStateEventData eventData)
        {
            if (eventData.Controller is MagicLeapMRTKController controller)
            {
                enabled = false;
            }
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
        }

        public void OnInputChanged(InputEventData<Vector2> eventData)
        {
            if (eventData.MixedRealityInputAction == AButtonAction)
            {
                APositionAction?.Invoke(eventData);
            } 
            if (eventData.MixedRealityInputAction == BButtonAction)
            {
                BPositionAction?.Invoke(eventData);
            } 
            if (eventData.MixedRealityInputAction == CButtonAction)
            {
                CPositionAction?.Invoke(eventData);
            } 
        }

        public void OnInputUp(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == AButtonAction)
            {
                aPress = false;
                AUpAction?.Invoke(eventData);
            }

            if (eventData.MixedRealityInputAction == BButtonAction)
            {
                bPress = false;
                BUpAction?.Invoke(eventData);
            }

            if (eventData.MixedRealityInputAction == CButtonAction)
            {
                cPress = false;
                CUpAction?.Invoke(eventData);
            }
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction == AButtonAction)
            {
                aPress = true;
                if (bPress)
                {
                    ABDownAction?.Invoke(eventData);
                    return;
                }

                if (cPress)
                {
                    ACDownAction?.Invoke(eventData);
                    return;
                }

                ADownAction?.Invoke(eventData);
            }

            if (eventData.MixedRealityInputAction == BButtonAction)
            {
                bPress = true;
                if (aPress)
                {
                    ABDownAction?.Invoke(eventData);
                    return;
                }

                if (cPress)
                {
                    BCDownAction?.Invoke(eventData);
                    return;
                }

                BDownAction?.Invoke(eventData);
            }

            if (eventData.MixedRealityInputAction == CButtonAction)
            {
                cPress = true;
                if (bPress)
                {
                    BCDownAction?.Invoke(eventData);
                    return;
                }

                if (aPress)
                {
                    ACDownAction?.Invoke(eventData);
                    return;
                }

                CDownAction?.Invoke(eventData);
            }
        }

        private void ResetAllActions()
        {
            ADownAction = null;
            AUpAction = null;
            APositionAction = null;
            BDownAction = null;
            BUpAction = null;
            BPositionAction = null;
            CUpAction = null;
            CPositionAction = null;
            ABDownAction = null;
            ACDownAction = null;
            BCDownAction = null;
        }
    }
}
