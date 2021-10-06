using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Renderer TargetRenderer { get { return targetRenderer; } }
    [SerializeField]
    private Renderer targetRenderer;

    private void OnEnable()
    {
        targetRenderer = GetComponent<Renderer>();
    }


    public void OnSliderUpdatedRed(SliderEventData eventData)
    {
        if ((targetRenderer != null) && (targetRenderer.material != null))
        {
            targetRenderer.material.color = new Color(eventData.NewValue, targetRenderer.sharedMaterial.color.g, targetRenderer.sharedMaterial.color.b, targetRenderer.sharedMaterial.color.a);
        }
    }

    public void OnSliderUpdatedGreen(SliderEventData eventData)
    {
        if ((targetRenderer != null) && (targetRenderer.material != null))
        {
            targetRenderer.material.color = new Color(targetRenderer.sharedMaterial.color.r, eventData.NewValue, targetRenderer.sharedMaterial.color.b, targetRenderer.sharedMaterial.color.a);
        }
    }

    public void OnSliderUpdateBlue(SliderEventData eventData)
    {
        if ((targetRenderer != null) && (targetRenderer.material != null))
        {
            targetRenderer.material.color = new Color(targetRenderer.sharedMaterial.color.r, targetRenderer.sharedMaterial.color.g, eventData.NewValue, targetRenderer.sharedMaterial.color.a);
        }
    }

    public void OnSliderUpdateAlpha(SliderEventData eventData)
    {
        if ((targetRenderer != null) && (targetRenderer.material != null))
        {
            targetRenderer.material.color = new Color(targetRenderer.sharedMaterial.color.r, targetRenderer.sharedMaterial.color.g, targetRenderer.sharedMaterial.color.b, eventData.NewValue);
        }
    }
}
