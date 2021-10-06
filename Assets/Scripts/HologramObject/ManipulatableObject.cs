using System;
using DataScripts;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

using Visualization;

namespace HologramObject
{
    public class ManipulatableObject : ObjectManipulator, IBaseHologramObject, IVisibleObject
    {
        public GameObject GameObject => gameObject;
        public HologramData HologramData => hologramData;
        public HologramEditor Editor { get; private set; }
        public ToolTip ToolTip { get; set; }
        public VisibleCondition VisibleCondition { get; set; }
        public Action<IVisibleObject, IVisualSettings> VisualizationAction { get; set; }
        public IVisualSettings VisualSettings { get; private set; }

        private HologramData hologramData;

        public void UpdateObject(HologramData data)
        {
            name = data.Id;
            transform.position = data.SpatialData.Position;
            transform.rotation = data.SpatialData.Rotation;
            transform.localScale = data.SpatialData.Scale;
            var renderer = GetComponentInParent<Renderer>();
            renderer.material.color = data.RendererData.Color;

            hologramData = data;
        }

        public void UpdateData()
        {
            var hologramRenderer = GetComponentInParent<Renderer>();
            hologramData = new HologramData(hologramData.PrefabName, hologramData.Id,
                new SpatialData(transform.position, transform.rotation, transform.localScale),
                new RendererData(hologramRenderer, hologramRenderer.material.color));
            Editor ??= GetComponent<HologramEditor>() ?? gameObject.AddComponent<HologramEditor>();
        }

        public void SetVisualSettings(IVisualSettings settings)
        {
            VisualSettings = settings;
        }

        private void Update()
        {
            if (VisibleCondition == VisibleCondition.Process)
            {
                VisualizationAction?.Invoke(this, VisualSettings);
            }
        }
    }
}
