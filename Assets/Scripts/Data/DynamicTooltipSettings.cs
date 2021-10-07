using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace DataScripts
{
    public readonly struct DynamicTooltipSettings : ITooltipSettings
    {
        public string Name { get; }
        public bool ShowConnector { get; }
        public ConnectorOrientType ConnectorOrientType { get; }
        public ConnectorFollowType ConnectorFollowType { get; }
        public ConnectorPivotMode ConnectorPivotMode { get; }
        public ConnectorPivotDirection ConnectorPivotDirection { get; }
        public float ConnectorPivotDistance { get; }

        public DynamicTooltipSettings(ITooltipSettings tooltipSettings)
        {
            Name = tooltipSettings.Name;
            ShowConnector = tooltipSettings.ShowConnector;
            ConnectorOrientType = tooltipSettings.ConnectorOrientType;
            ConnectorFollowType = tooltipSettings.ConnectorFollowType;
            ConnectorPivotMode = tooltipSettings.ConnectorPivotMode;
            ConnectorPivotDirection = tooltipSettings.ConnectorPivotDirection;
            ConnectorPivotDistance = tooltipSettings.ConnectorPivotDistance;
        }

        public DynamicTooltipSettings(
            string name = "",
            bool showConnector = true,
            ConnectorOrientType connectorOrientType = ConnectorOrientType.OrientToCamera,
            ConnectorFollowType connectorFollowType = ConnectorFollowType.Position,
            ConnectorPivotMode connectorPivotMode = ConnectorPivotMode.Automatic,
            ConnectorPivotDirection connectorPivotDirection = ConnectorPivotDirection.West,
            float connectorPivotDistance = .3f)
        {
            Name = name;
            ShowConnector = showConnector;
            ConnectorOrientType = connectorOrientType;
            ConnectorFollowType = connectorFollowType;
            ConnectorPivotMode = connectorPivotMode;
            ConnectorPivotDirection = connectorPivotDirection;
            ConnectorPivotDistance = connectorPivotDistance;
        }
    }
}
