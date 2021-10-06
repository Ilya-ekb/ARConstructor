using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using Serialization;
using UnityEngine;

public interface IBaseHologramObject
{
    GameObject GameObject { get; }
    HologramData HologramData { get; }
    HologramEditor Editor { get; }
    ToolTip ToolTip { get; set; }
    void UpdateObject(HologramData data);
    void UpdateData();
}
