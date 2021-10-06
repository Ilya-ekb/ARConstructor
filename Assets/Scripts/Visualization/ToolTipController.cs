using DataScripts;
using Microsoft.MixedReality.Toolkit.UI;
using MRTK;
using UnityEngine;

namespace Visualization
{
    public class ToolTipController : VisualController
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

            if (childVisualizer && currentVisibleCondition == VisibleCondition.Visible)
            {
                TooltipVisualizer.Instance.Decorate(childVisualizer, childVisualizer.VisualSettings);
            }

            foreach (var hologram in objectCreator.ContainedBaseHologramObjects)
            {
                if (currentVisibleCondition == VisibleCondition.Visible)
                {
                    TooltipVisualizer.Instance.SetVisible(hologram, TooltipVisualizer.Instance.VisualSettings);
                }
                else
                {
                    TooltipVisualizer.Instance.SetInvisible(hologram, TooltipVisualizer.Instance.VisualSettings);
                }
            }
        }
    }
}

