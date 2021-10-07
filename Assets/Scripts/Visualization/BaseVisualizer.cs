using System;
using System.Collections.Generic;
using DataScripts;
using Main;
using UnityEngine;
using UnityEngine.XR;

namespace Visualization
{
    public abstract class BaseVisualizer<TSettings> : Singleton<BaseVisualizer<TSettings>>, IVisualizer where TSettings : IVisualSettings
    {
        public IVisualizer Visualizer { get; private set; }
        protected abstract TSettings staticVisualSettings { get; }

        private readonly Dictionary<IVisibleObject, TSettings> settingsMap = new Dictionary<IVisibleObject, TSettings>();

        protected virtual TSettings GetVisualSettings(IVisibleObject visibleObject)
        {
            return settingsMap.ContainsKey(visibleObject) ? settingsMap[visibleObject] : staticVisualSettings;
        }

        public virtual void SetVisualSettings(IVisibleObject visibleObject, IVisualSettings settings = null)
        {
            var resultSettings = settings == null ? staticVisualSettings : (TSettings)settings;

            if (settingsMap.ContainsKey(visibleObject))
            {
                settingsMap[visibleObject] = resultSettings;
            }
            else
            {
                settingsMap.Add(visibleObject, resultSettings);
            }
        }

        public void Decorate<T>(IVisualizer visualizer, IVisibleObject visibleObject, T settings = default) where T : IVisualSettings
        {
            Visualizer = visualizer;
            Visualizer?.SetVisualSettings(visibleObject, settings);
        }

        public void SetVisible(object visibleObject, IVisualSettings visualSettings = null)
        {
            if (!(visibleObject is IVisibleObject vObject))
            {
                return;
            }

            if (visualSettings != null)
            {
                SetVisualSettings(vObject, visualSettings);
            }

            vObject.SetVisualSettings(GetVisualSettings(vObject));

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

            vObject.SetVisualSettings(GetVisualSettings(vObject));

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

