using DataScripts;

namespace Visualization
{
    public interface IVisualizer : IVisualization
    {
        IVisualizer Visualizer { get; }
        IVisualSettings VisualSettings { get; }
        void Decorate<T>(IVisualizer visualizer, T settings) where T : IVisualSettings;
        void SetVisible(object hologramObject, IVisualSettings visualSettings = null);
        void SetInvisible(object hologramObject, IVisualSettings visualSettings = null);
        void SetInvisibleAll();
    }
}
