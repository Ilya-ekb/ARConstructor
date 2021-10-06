using DataScripts;

namespace Visualization
{
    public interface IVisualization
    {
        IVisualSettings VisualSettings { get; }

        void SetVisualSettings(IVisualSettings settings);

    }
}
