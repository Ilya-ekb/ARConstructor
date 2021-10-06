using System;
using DataScripts;

namespace Visualization
{
    public interface IVisibleObject : IVisualization
    {
        VisibleCondition VisibleCondition { get; set; }
        Action<IVisibleObject, IVisualSettings> VisualizationAction { get; set; }
    }
}

