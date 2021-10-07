using DataScripts;
using Microsoft.MixedReality.Toolkit.UI;
using MRTK;
using UnityEngine;

namespace Visualization
{
    [RequireComponent(typeof(Interactable))]
    public class SmoothController : VisualController
    {
        public Interactable Interactable { get; set; }

        [SerializeField] private BaseVisualizer<ITooltipSettings> childVisualizer;
        [SerializeField] private ObjectCreator objectCreator;
        private VisibleCondition currentVisibleCondition = VisibleCondition.Invisible;

        private void OnEnable()
        {
            if (Interactable == null)
            {
                Interactable = GetComponent<Interactable>();
            }
            Turn();
        }

        private void OnDisable()
        {
            Turn();
        }

        public override void Turn()
        {
            currentVisibleCondition = Interactable.IsToggled ? VisibleCondition.Visible : VisibleCondition.Invisible;

            if (objectCreator == null)
            {
                return;
            }


            foreach (var hologram in objectCreator.ContainedBaseHologramObjects)
            {
                if (currentVisibleCondition == VisibleCondition.Visible)
                {
                    if (childVisualizer)
                    {
                        SmoothAlphaVisualizer.Instance.Decorate(childVisualizer, hologram as IVisibleObject, default(ISmoothSettings));
                    }

                    SmoothAlphaVisualizer.Instance.SetVisible(hologram);
                }
                else
                {
                    if (childVisualizer)
                    {
                        SmoothAlphaVisualizer.Instance.Decorate(childVisualizer, hologram as IVisibleObject, default(ISmoothSettings));
                    }
                    SmoothAlphaVisualizer.Instance.SetInvisible(hologram);
                }
            }
        }
    }
}