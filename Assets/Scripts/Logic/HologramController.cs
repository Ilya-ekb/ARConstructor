using System.Collections;
using DataScripts;
using Microsoft.MixedReality.Toolkit.UI;
using Serialization;
using UnityEngine;
using Visualization;

public class HologramController: Singleton<HologramController>
{
    public IVisualizer HologramObjectsSmoothAlpha { get; set; }
}
