using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DataScripts;
using Main;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Visualization
{
    public class TooltipVisualizer : BaseVisualizer<ITooltipSettings>
    {
        [SerializeField] private GameObject tooltipPrefab;
        [SerializeField, HideInInspector] private string resourcePath;
        private Object loadedPrefab;
        

        private readonly Dictionary<IBaseHologramObject, ToolTip> hologramObjectTooltipsMap =
            new Dictionary<IBaseHologramObject, ToolTip>();

        protected override ITooltipSettings staticVisualSettings => Settings.Instance.StaticTooltipSettings;


        private void Start()
        {
            if (tooltipPrefab == null)
            {
                enabled = false;
                return;
            }
            loadedPrefab = Resources.Load(resourcePath);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            resourcePath = GetPrefabLink(AssetDatabase.GetAssetPath(tooltipPrefab));
            string GetPrefabLink(string assetPath)
            {
                string result = string.Empty;
                var pathArray = assetPath.Split('/');
                for (var i = 2; i < pathArray.Length - 1; i++)
                {
                    result += pathArray[i] + "/";
                }

                result += pathArray[pathArray.Length - 1].Split('.')[0];
                return result;
            }
#endif
        }

        protected override void ChangeState(IVisibleObject visibleObject, ITooltipSettings settings, VisibleCondition targetCondition)
        {
            if (!(visibleObject is IBaseHologramObject hologramObject))
            {
                return;
            }

            ToolTip toolTip;

            if (!hologramObjectTooltipsMap.ContainsKey(hologramObject))
            {

                toolTip = ((GameObject) Instantiate(loadedPrefab)).GetComponent<ToolTip>();
                if (toolTip == null)
                {
                    return;
                }
                hologramObjectTooltipsMap.Add(hologramObject, toolTip);
            }

            toolTip = hologramObjectTooltipsMap[hologramObject];
            toolTip.gameObject.SetActive(targetCondition == VisibleCondition.Visible);

            if (!toolTip.gameObject.activeSelf)
            {
                visibleObject.VisibleCondition = VisibleCondition.Invisible;
                visibleObject.VisualizationAction -= TurnOffVisualization;
                Visualizer?.SetInvisible(visibleObject);
                return;
            }


            toolTip.ToolTipText = settings.Name == Settings.Instance.StaticTooltipSettings.Name ? settings.Name + hologramObject.HologramData.Id : settings.Name;

            toolTip.ShowConnector = settings.ShowConnector;
            var connector = toolTip.GetComponent<ToolTipConnector>();
            connector.Target = hologramObject.GameObject;

            connector.PivotDirectionOrient = settings.ConnectorOrientType;
            connector.ConnectorFollowingType = settings.ConnectorFollowType;
            connector.PivotMode = settings.ConnectorPivotMode;
            connector.PivotDirection = settings.ConnectorPivotDirection;
            connector.PivotDistance = settings.ConnectorPivotDistance;
            visibleObject.VisibleCondition = VisibleCondition.Visible;
            visibleObject.VisualizationAction -= TurnOnVisualization;
            Visualizer?.SetVisible(visibleObject);
        }
    }
}
