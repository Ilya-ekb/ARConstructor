using System.Collections.Generic;
using System.Linq;
using DataScripts;
using Microsoft.MixedReality.Toolkit.Input;
using Serialization;
using Storage;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MRTK
{
    public class ObjectCreator : MonoBehaviour, IBaseHologramObjectContainer
    {
        public string Id => gameObject.name;
        public IEnumerable<string> ContainedIds => containedObjectsId;

        public IEnumerable<IBaseHologramObject> ContainedBaseHologramObjects =>
            Data.Instance.AllBaseHologramObjects.Where(e => containedObjectsId.Any(id => id == e.HologramData.Id));

        [SerializeField] private BaseInputHandler baseInputHandler;
        [SerializeField] private GameObject creatingObjectPrefab;
        [SerializeField, HideInInspector] private string resourcePath;
        private Object loaderPrefab;

        private readonly List<string> containedObjectsId = new List<string>();

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (creatingObjectPrefab == null)
            {
                return;
            }
            resourcePath = GetPrefabLink(AssetDatabase.GetAssetPath(creatingObjectPrefab));

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

        private void OnEnable()
        {
            if (baseInputHandler == null)
            {
                return;
            }

            var holoObjects = ContainedBaseHologramObjects;

            baseInputHandler.ADownAction += CreateEvent;
            baseInputHandler.ABDownAction += DeleteEvent;
        }

        private void OnDisable()
        {
            if (baseInputHandler == null)
            {
                return;
            }
            var holoObjects = ContainedBaseHologramObjects;

            baseInputHandler.ADownAction -= CreateEvent;
            baseInputHandler.ABDownAction -= DeleteEvent;
        }

        public IBaseHologramObject RestoreHologramObjectEvent(Memo memo)
        {
            loaderPrefab = Resources.Load(resourcePath);

            if (memo == null)
            {
                return null;
            }

            var data = new HologramData(memo);
            return CreateObject(data);
        }

        private void CreateEvent(InputEventData eventData)
        {
            var interactable = eventData.InputSource.Pointers[0]?.Result.CurrentPointerTarget?
                .GetComponent<IMixedRealityFocusChangedHandler>();
            if (interactable != null)
            {
                return;
            }

            var spatialData = new SpatialData(
                eventData.InputSource.Pointers[0].BaseCursor.Position,
                eventData.InputSource.Pointers[0].BaseCursor.Rotation,
                creatingObjectPrefab.transform.localScale);

            var hologramData = new HologramData(creatingObjectPrefab.name, null, spatialData);

            CreateObject(hologramData);
        }
        
        private void DeleteEvent(InputEventData eventData)
        {
            var id = eventData.InputSource?.Pointers[0]?.Result.CurrentPointerTarget?.name;
            Serializator.Delete(id);
            DeleteObject(id);
        }

        private IBaseHologramObject CreateObject(HologramData hologramData)
        {
            if (creatingObjectPrefab == null)
            {
                return null;
            }

            var gameObject = (GameObject)Instantiate(loaderPrefab);
            var renderer = gameObject.GetComponentInParent<Renderer>();
            var color = hologramData.RendererData.Color == default && renderer?.material != null
                ? renderer.material.color
                : hologramData.RendererData.Color;

            var rendererData = new RendererData(renderer, color);

            hologramData = new HologramData(hologramData.PrefabName, hologramData.Id, hologramData.SpatialData, rendererData);

            var baseHologram = gameObject.GetComponent<IBaseHologramObject>();

            baseHologram.UpdateObject(hologramData);

            containedObjectsId.Add(baseHologram.HologramData.Id);
            Data.Instance.AddNewBaseHologramObject(baseHologram);
            return Data.Instance.GetBaseHologramObject(baseHologram.HologramData.Id);
        }

        private void DeleteObject(string id)
        {
            if (string.IsNullOrEmpty(id) || Data.Instance.GetBaseHologramObject(id) == null)
            {
                return;
            }

            containedObjectsId.Remove(id);
            Data.Instance.RemoveHologramObject(id);
        }
    }
}
