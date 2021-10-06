using DataScripts;
using UnityEngine;

namespace Main
{
    public class Settings : Singleton<Settings>
    {
        public VisualizationSmoothAlphaSettings HologramVisualizationSmoothAlpha => hologramVisualizationSmoothAlpha;
        public StaticTooltipSettings StaticTooltipSettings => staticTooltipSettings;

        [SerializeField] private VisualizationSmoothAlphaSettings hologramVisualizationSmoothAlpha;
        [SerializeField] private StaticTooltipSettings staticTooltipSettings;
    }
}
