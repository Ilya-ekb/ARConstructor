using DataScripts;

namespace Visualization
{
    public interface IVisualizer
    {
        IVisualizer Visualizer { get; }
        void Decorate<T>(IVisualizer visualizer,IVisibleObject visibleObject, T settings) where T : IVisualSettings;
        void SetVisible(object hologramObject, IVisualSettings visualSettings = null);
        void SetInvisible(object hologramObject, IVisualSettings visualSettings = null);
        void SetVisualSettings(IVisibleObject visibleObject, IVisualSettings visualSettings = null);
        void SetInvisibleAll();
    }
}
