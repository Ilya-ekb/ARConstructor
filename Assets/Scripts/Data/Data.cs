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

        public static int ID { get; private set; } = 0;

        public GameObject scrollObject;
        public AudioClip[] audioClips;
        public Sprite[] images;

        public void AddHologramObject(IBaseHologramObject baseHologramObject)
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

        public IBaseHologramObject GetHologramObject(string id)
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
            ID = 0;
            var directoryInfos = new List<DirectoryInfo>
            {
                new DirectoryInfo(Application.persistentDataPath),
                new DirectoryInfo(filePath),
                new DirectoryInfo(Path.Combine(filePath, "SingleData"))
            };
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

    public enum ButtonType
    {
        ButtonYes,
        ButtonNo,
        ButtonOk,
        ButtonCancel
    }
}



