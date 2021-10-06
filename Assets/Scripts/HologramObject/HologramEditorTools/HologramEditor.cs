using System.Collections;
using System.Collections.Generic;
using DataScripts;
using HologramObject;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;

public class HologramEditor : InputActionHandler
{
    public BoundsControl BoundsControl => boundsControl;
    private BoundsControl boundsControl = null;

    public ManipulatableObject ObjectManipulator => objectManipulator;
    private ManipulatableObject objectManipulator = null;
    public ChangeColor ChangeColor { get { if (changerColor == null) changerColor = GetComponent<ChangeColor>(); return changerColor; } }
    private ChangeColor changerColor = null;
    protected IBaseHologramObject[] objects;

    protected override void OnEnable()
    {
        base.OnEnable();
        InputAction = MixedRealityInputAction.Select;
        //StartSettings();
    }

    protected override void OnActionStart()
    {
        StartEdit();
    }

    /// <summary>
    /// Включение редактирования объекта
    /// </summary>
    protected virtual void StartEdit()
    {
        StartSettings();
        ResetStateAll();
        Data.ObjectFeaturePanelControl.CallFor(this);
        enabled = false;
    }

    /// <summary>
    /// Стартовые настройки объекта для редактирования
    /// </summary>
    protected virtual void StartSettings()
    {

        objectManipulator = gameObject.GetComponent<ManipulatableObject>();
        if (!objectManipulator)
        {
            objectManipulator = gameObject.AddComponent<ManipulatableObject>();
        }

        if (!boundsControl || !objectManipulator)
        {
            boundsControl = gameObject.GetComponent<BoundsControl>();
            if (!boundsControl)
            {
                boundsControl = gameObject.AddComponent<BoundsControl>();
                boundsControl.ScaleHandlesConfig.ScaleBehavior = Microsoft.MixedReality.Toolkit.UI
                    .BoundsControlTypes.HandleScaleMode.Uniform;
            }
        }

        boundsControl.enabled = false;
        objectManipulator.enabled = false;
    }

    /// <summary>
    ///  Set <see cref="HologramEditor "/> to initial condition 
    /// </summary>
    public static void ResetStateAll()
    {
        Data.ObjectFeaturePanelControl.gameObject.SetActive(false);
        foreach (var obj in Data.Instance.AllBaseHologramObjects)
        {
            if (obj == null)
            {
                continue;
            }
            obj.UpdateData();

            if (obj.Editor == null)
            {
                continue;
            }
            var bound = obj.Editor.BoundsControl;
            var manipulator = obj.Editor.ObjectManipulator;
            if (bound) bound.enabled = false;
            if (manipulator) manipulator.enabled = false;
            obj.Editor.enabled = true;
        }
    }
}
