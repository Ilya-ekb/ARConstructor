using UnityEngine;

namespace DataScripts
{
    [CreateAssetMenu(fileName = "StaticSmoothAlphaVisualSettings", menuName= "Hologram/Visualization Settings/Static Smooth Alpha Setting")]
    public class VisualizationSmoothAlphaSettings : ScriptableObject, ISmoothSettings
    {
        public float AlphaThreshold => alphaThreshold;
        public float SmoothSpeed => smoothSpeed;
        public float VisibleAlpha => visibleAlpha;
        public float InvisibleAlpha => invisibleAlpha;

        [SerializeField] private float alphaThreshold = .05f;
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private float visibleAlpha = 1.0f;
        [SerializeField] private float invisibleAlpha = .0f;
    }
}
