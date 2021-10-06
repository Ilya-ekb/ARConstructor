using System;
using DataScripts;
using Main;
using UnityEngine;

namespace Visualization
{
    public class SmoothAlphaVisualizer : BaseVisualizer<ISmoothSettings>
    {
        protected override IVisualSettings visualSettings
        {
            get => (IVisualSettings) smoothSettings ?? Settings.Instance.HologramVisualizationSmoothAlpha;
            set => SetVisualSettings(value);
        }

        private ISmoothSettings smoothSettings;

        public override void SetVisualSettings(IVisualSettings settings)
        {
            smoothSettings = settings as ISmoothSettings;
        }

        protected override void ChangeState(IVisibleObject visibleObject, ISmoothSettings settings, VisibleCondition targetCondition)
        {
            if (!(visibleObject is IBaseHologramObject baseHologram))
            {
                return;
            }

            var targetAlpha = targetCondition == VisibleCondition.Invisible
                ? settings.InvisibleAlpha
                : settings.VisibleAlpha;
            var data = baseHologram.HologramData;
            var rendererData = data.RendererData;
            var color = rendererData.Color;

            if (Math.Abs(rendererData.Color.a - targetAlpha) > settings.AlphaThreshold)
            {
                color.a = Mathf.MoveTowards(color.a, targetAlpha,
                    Time.deltaTime * settings.SmoothSpeed);
            }
            else
            {
                color.a = targetAlpha;
            }

            if (Mathf.Approximately(rendererData.Color.a, color.a))
            {

                if (Math.Abs(rendererData.Color.a - settings.VisibleAlpha) <= settings.AlphaThreshold)
                {
                    visibleObject.VisibleCondition = VisibleCondition.Visible;
                    visibleObject.VisualizationAction -= TurnOnVisualization;
                    Visualizer?.SetVisible(visibleObject, Visualizer.VisualSettings);

                }
                else
                {
                    visibleObject.VisibleCondition = VisibleCondition.Invisible;
                    visibleObject.VisualizationAction -= TurnOffVisualization;
                    Visualizer?.SetInvisible(visibleObject, Visualizer.VisualSettings);
                }

            }

            rendererData.Color = color;

            data = new HologramData(data.PrefabName, data.Id, data.SpatialData, rendererData);
            baseHologram.UpdateObject(data);
        }
    }
}
