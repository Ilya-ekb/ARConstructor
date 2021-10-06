using System.Collections;
using Microsoft.MixedReality.Toolkit.UI;

namespace DataScripts
{
    public interface ITooltipSettings : IVisualSettings
    {
        string Name { get; }
        bool ShowConnector { get; }
        ConnectorOrientType ConnectorOrientType { get; }
        ConnectorFollowType ConnectorFollowType { get; }
        ConnectorPivotMode ConnectorPivotMode { get; }
        ConnectorPivotDirection ConnectorPivotDirection { get; }
        float ConnectorPivotDistance { get; }
    }
}
