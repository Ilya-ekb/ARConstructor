using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

using System.Collections;
using System.Collections.Generic;
using DataScripts;
using UnityEngine;

public class ObjectFeaturePanelControl : MonoBehaviour
{
    [SerializeField] private Interactable[] toggles;
    [SerializeField] private Interactable[] sizeMoveToggles;
    [SerializeField] private GameObject contentContainer;
    private HologramEditor chosedObject;
    private HologramEditor editingObject;
    private RadialView radialView;

    private void OnEnable()
    {
        radialView = GetComponent<RadialView>();
        foreach (var toggle in toggles)
        {
            toggle.OnClick.AddListener(() =>
            {
                InterfaceSwitcher(toggle);
            });
        }
    }

    /// <summary>
    /// Вызов панели настройки для объекта редактирования
    /// </summary>
    /// <param name="editingObject"></param>
    public void CallFor(HologramEditor editingObject, Transform parent = null)
    {

        radialView.enabled = true;
        this.editingObject = chosedObject = editingObject;
        gameObject.SetActive(false);
        transform.parent = null;
        transform.localScale = Vector3.one;
        for (var child = 0; child < contentContainer.transform.childCount; child++)
        {
            var interFace = contentContainer.transform.GetChild(child).GetComponent<IFeatureInterface>();
            interFace.Dispose();
            interFace.Setting(editingObject);
        }
        foreach (var button in sizeMoveToggles)
        {
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener(() =>
            {
                if (button.gameObject.GetComponent<ButtonConfigHelper>().MainLabelText == "Moving")
                {
                    if (button.IsToggled)
                    {
                        editingObject.ObjectManipulator.enabled = true;
                        if (editingObject.BoundsControl) editingObject.BoundsControl.enabled = false;
                    }
                    else
                    {
                        editingObject.ObjectManipulator.enabled = false;
                    }
                }
                else if (button.gameObject.GetComponent<ButtonConfigHelper>().MainLabelText == "Edit")
                {
                    if (button.IsToggled)
                    {
                        editingObject.ObjectManipulator.enabled = false;
                        if (editingObject.BoundsControl) editingObject.BoundsControl.enabled = true;
                    }
                    else
                    {
                        if (editingObject.BoundsControl) editingObject.BoundsControl.enabled = false;
                    }
                }
            });
        }
        gameObject.SetActive(true);
        radialView.enabled = true;
    }

    public void DeleteChoosedObject()
    {
        //if (chosedObject == editingObject) Off();
        //Data.InfoObjects.Dispose();
    }

    public void Off()
    {
        radialView.enabled = false;
        if (gameObject && gameObject.activeSelf) gameObject.SetActive(false);
    }


    /// <summary>
    /// Переключение интерфейса
    /// </summary>
    /// <param name="toggle"></param>
    public void InterfaceSwitcher(Interactable toggle)
    {
        //foreach (var tog in toggles)
        //{
        //    if (tog.name == toggle.name)
        //    {
        //        tog.GetComponent<InterfaceControl>()?.Activate();
        //    }
        //    else
        //    {
        //        tog.GetComponent<InterfaceControl>()?.Deactivate();
        //    }
        //}
    }
}
