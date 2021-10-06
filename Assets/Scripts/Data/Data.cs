using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using LogicScripts;
using Serialization;
using Storage;

namespace DataScripts
{
    public class Data : Singleton<Data>
    {
        [ThreadStatic]
        public static readonly bool IsMainThread = true;
        public IEnumerable <IBaseHologramObject> AllBaseHologramObjects => allBaseHologramObjects.Values;

        private Dictionary<string, IBaseHologramObject> allBaseHologramObjects { get; } = new Dictionary<string, IBaseHologramObject>();

        public static ObjectFeaturePanelControl ObjectFeaturePanelControl => objectFeaturePanelControl;

        internal static Logic Logic { get; set; }

        public static int ID { get; private set; } = 0;

        private static ObjectFeaturePanelControl objectFeaturePanelControl;

        public GameObject objectFeaturePanelControlPrefab;
        public GameObject spatial;
        public GameObject uIDialogPrefab;
        public GameObject scrollObject;
        public AudioClip[] audioClips;
        public Sprite[] images;

        public Type Type;

        public void Update_ID()
        {
            ID++;
        }

        private void Awake()
        {
            ////objectFeaturePanelControl = Instantiate(objectFeaturePanelControlPrefab, Camera.main.transform).GetComponent<ObjectFeaturePanelControl>();
            //objectFeaturePanelControl.gameObject.SetActive(false);
            //Instantiate(uIDialogPrefab);
            //objectFeaturePanelControl.gameObject.SetActive(false);

            //ID = Serializator.Deserialization<int>("ID");
            //var int_values = new int[Scenario1Obj.Count];
            //for (var i = 0; i < int_values.Length; i++)
            //{
            //    var key = Scenario1Obj.ElementAt(i).Key;
            //    int value = Serializator.Deserialization<int>(key);
            //    Scenario1Obj.Remove(key);
            //    Scenario1Obj.Add(key, value);
            //}
        }
        public void StartScenario()
        {
           // scenarios[1].StartScenario();
        }

        public void AddNewBaseHologramObject(IBaseHologramObject baseHologramObject)
        {
            if (allBaseHologramObjects.ContainsKey(baseHologramObject.HologramData.Id))
            {
                return;
            }
            allBaseHologramObjects.Add(baseHologramObject.HologramData.Id, baseHologramObject);
        }

        public void RemoveHologramObject(string id)
        {
            if (!allBaseHologramObjects.ContainsKey(id))
            {
                return;
            }
            Destroy(allBaseHologramObjects[id].GameObject);
            allBaseHologramObjects.Remove(id);
        }

        public IBaseHologramObject GetBaseHologramObject(string id)
        {
            return allBaseHologramObjects.ContainsKey(id) ? allBaseHologramObjects[id] : null;
        }

        public void ResetAll()
        {
            foreach (var obj in allBaseHologramObjects.Values.Where(obj => obj != null).Where(obj => obj.GameObject))
            {
                Destroy(obj.GameObject);
            }
            allBaseHologramObjects.Clear();
            var filePath = Path.Combine(Application.persistentDataPath, "Data");
            var data = new DirectoryInfo(Application.persistentDataPath);
            var dataInfo = new DirectoryInfo(filePath);
            var dataSingle = new DirectoryInfo(Path.Combine(filePath, "SingleData"));
            ID = 0;
            var directoryInfos = new List<DirectoryInfo>();
            directoryInfos.Add(data);
            directoryInfos.Add(dataInfo);
            directoryInfos.Add(dataSingle);
            foreach (var file in directoryInfos.Select(info => info.GetFiles()).SelectMany(fileInfo => fileInfo))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Cannot delete file: {file.Name} \nLog: {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// Типы создаваемых объектов
    /// </summary>
    public enum ObjectType
    {
        Tooltip,
        Point,
        WorldAnchor,
        Empty,
    }
    public enum PointType
    {
        panel_to_floor_,
        tooltip_,
        point_,
        capsule_,
        world_anchor_,
        empty_,
    }

    /// <summary>
    /// Типы кнопок
    /// </summary>
    public enum ButtonType
    {
        ButtonYes,
        ButtonNo,
        ButtonOk,
        ButtonCancel
    }
}



