using System.Linq;
using DataScripts;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Storage;
using UnityEngine;
using Visualization;

namespace LogicScripts
{
    public class SceneOrganizer : Singleton<SceneOrganizer>
    {

        private IPersistentDataController storageController;

        private TextMesh lastLabelPlacedText;
        
        private float probabilityThreshold = .2f;

        private Renderer quadRenderer;

        private Transform lastLabelPlaced;

        private ScrollList scrollList;

        private AudioSource audioSource;
        private IBaseHologramObject[] allHolograms;

        private void Awake()
        {
            gameObject.AddComponent<HologramController>();
        }

        private void OnEnable()
        {
            StorageController.Instance.Load();
            allHolograms = Data.Instance.AllBaseHologramObjects.ToArray();
            SmoothAlphaVisualizer.Instance.SetInvisibleAll();
        }

        public TextMesh CreateUI(Transform parent, string name, float scale, float yPos, float zPos, bool setActive)
        {
            var display = new GameObject(name, typeof(TextMesh));
            display.transform.parent = parent;
            display.transform.localPosition = new Vector3(.0f, yPos, zPos);
            display.SetActive(setActive);
            display.transform.localRotation = new Quaternion();
            display.transform.localScale = new Vector3(scale, scale, scale);
            var textMesh = display.GetComponent<TextMesh>();
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            return textMesh;
        }

        /// <summary>
        /// Вызов указателя к действию
        /// </summary>
        public void Pointer(params object[] vs)
        {
            if (vs.Length <= 0)
            {
                return;
            }

            var hologramObject = allHolograms[(int)vs[1]];
            if (!string.IsNullOrEmpty(vs[2].ToString()))
            {
                var tooltipSettings = new DynamicTooltipSettings(vs[2].ToString());
                if (vs.Length > 3)
                {
                    tooltipSettings = new DynamicTooltipSettings(
                        vs[2].ToString(),
                        (bool) vs[3],
                        (ConnectorOrientType) vs[4],
                        (ConnectorFollowType) vs[5],
                        (ConnectorPivotMode) vs[6],
                        (ConnectorPivotDirection) vs[7],
                        (float) vs[8]);
                }

                if (vs.Length > 9)
                {
                    var touchEvent = hologramObject.GameObject.GetComponent<HologramObject.TouchEvent>();
                    if (touchEvent != null )
                    {
                        touchEvent.OnTouchEvent.RemoveAllListeners();
                        touchEvent.OnTouchEvent.AddListener(() => touchEvent.InvokeScenarioMethod(vs[9].ToString()));
                    }
                }
                SmoothAlphaVisualizer.Instance.Decorate(TooltipVisualizer.Instance, hologramObject as IVisibleObject, tooltipSettings);
            }

            SmoothAlphaVisualizer.Instance.SetVisible(hologramObject);
        }

        /// <summary>
        /// Вызов панели выбора
        /// </summary>
        /// <param name="vs"></param>
        public void ChoosePanel(params object[] vs)
        {
            if (vs.Length > 0)
            {
                int offset = 1;

                scrollList = Instantiate(Data.Instance.ScrollObject).GetComponent<ScrollList>();
                var radial = scrollList.gameObject.GetComponent<RadialView>();
                radial.MaxViewDegrees = 10.0f;
                scrollList.gameObject.transform.localScale = Vector3.zero;
                var parameters = new object[vs.Length - offset];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = vs[i + offset];
                }
                scrollList.Activate(parameters);
            }
        }

        /// <summary>
        /// Вызов аудио подсказки
        /// </summary>
        /// <param name="vs"></param>
        public void PlayAudio(params object[] vs)
        {
            if (!audioSource)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (Data.Instance.audioClips.Length <= (int) vs[1])
            {
               return;
            }
            audioSource.clip = Data.Instance.audioClips[(int)vs[1]];
            audioSource.Play();
        }
    }
}
