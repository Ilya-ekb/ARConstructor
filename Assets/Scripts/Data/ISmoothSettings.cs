using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataScripts
{
    public interface ISmoothSettings : IVisualSettings
    {
        float AlphaThreshold { get; }
        float SmoothSpeed { get; }
        float VisibleAlpha { get; }
        float InvisibleAlpha { get; }
    }
}
