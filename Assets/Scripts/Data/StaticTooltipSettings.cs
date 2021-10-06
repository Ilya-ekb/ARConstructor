using Microsoft.MixedReality.Toolkit.UI;
using Unity.Profiling;
using UnityEngine;

namespace DataScripts
{
    [CreateAssetMenu(fileName = "StaticTooltipSettings", menuName = "Hologram/Visualization Settings/Static Tooltip Setting")]
    public class StaticTooltipSettings : ScriptableObject, ITooltipSettings
    {
        public string Name => name;
        public bool ShowConnector => showConnector;
        public ConnectorOrientType ConnectorOrientType => connectorOrientType;
        public ConnectorFollowType ConnectorFollowType => connectorFollowType;
        public ConnectorPivotMode ConnectorPivotMode => connectorPivotMode;
        public ConnectorPivotDirection ConnectorPivotDirection => connectorPivotDirection;
        public float ConnectorPivotDistance => connectorPivotDistance;

        [SerializeField] private string name = "";
        [SerializeField] private bool showConnector = true;
        [SerializeField] private ConnectorOrientType connectorOrientType = ConnectorOrientType.OrientToCamera;
        [SerializeField] private ConnectorFollowType connectorFollowType = ConnectorFollowType.PositionAndXYRotation;
        [SerializeField] private ConnectorPivotMode connectorPivotMode = ConnectorPivotMode.Automatic;
        [SerializeField] private ConnectorPivotDirection connectorPivotDirection = ConnectorPivotDirection.West;
        [SerializeField] private float connectorPivotDistance = .1f;
    }
}
