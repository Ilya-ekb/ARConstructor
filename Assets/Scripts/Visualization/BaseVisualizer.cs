using System;
using DataScripts;
using Main;
using UnityEngine;
using UnityEngine.XR;

namespace Visualization
{
    public abstract class BaseVisualizer<TSettings> : Singleton<BaseVisualizer<TSettings>>, IVisualizer where TSettings : IVisualSettings
    {
        public IVisualizer Visualizer { get; private set; }

        public IVisualSettings VisualSettings => visualSettings;

        protected abstract IVisualSettings visualSettings { get; set; }

        public void Decorate<T>(IVisualizer visualizer, T settings) where T : IVisualSettings
        {
            Visualizer = visualizer;
            Visualizer?.SetVisualSettings(settings);
        }

        public void SetVisible(object visibleObject, IVisualSettings visualSettings = null)
        {
            if (!(visibleObject is IVisibleObject vObject))
            {
                return;
            }

            vObject.SetVisualSettings(visualSettings ?? VisualSettings);

            vObject.VisibleCondition = VisibleCondition.Process;
            vObject.VisualizationAction -= TurnOffVisualization;
            vObject.VisualizationAction += TurnOnVisualization;
        }

        public void SetInvisible(object visibleObject, IVisualSettings visualSettings = null)
        {
            if (!(visibleObject is IVisibleObject vObject))
            {
                return;
            }

            vObject.SetVisualSettings(visualSettings ?? VisualSettings);

            vObject.VisibleCondition = VisibleCondition.Process;
            vObject.VisualizationAction -= TurnOnVisualization;
            vObject.VisualizationAction += TurnOffVisualization;
        }

        public void SetInvisibleAll()
        {
            var allObjects = Data.Instance.AllBaseHologramObjects;
            foreach (var baseHologramObject in allObjects)
            {
                if (baseHologramObject is IVisibleObject visibleObject)
                {
                    SetInvisible(visibleObject);
                }
            }
        }
        public abstract void SetVisualSettings(IVisualSettings settings);

        protected virtual void TurnOnVisualization(IVisibleObject visibleObject, IVisualSettings visualSettings)
        {
            if (!(visualSettings is TSettings settings))
            {
                enabled = false;
                Debug.LogError($"{name} {GetType().Name} in TurnOnVisualization settings is null");
                return;
            }

            ChangeState(visibleObject, settings, VisibleCondition.Visible);
        }

        protected virtual void TurnOffVisualization(IVisibleObject visibleObject, IVisualSettings visualSettings)
        {
            if (!(visualSettings is TSettings settings))
            {
                enabled = false;
                Debug.LogError($"{(visibleObject as IBaseHologramObject)?.GameObject.name} {GetType().Name} in TurnOffVisualization settings is null");
                return;
            }

            ChangeState(visibleObject, settings, VisibleCondition.Invisible);
        }

        protected abstract void ChangeState(IVisibleObject visibleObject, TSettings settings,
            VisibleCondition targetCondition);
    }
}

