using System.Collections.Generic;
using System.Linq;
using DataScripts;
using Serialization;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Storage
{
    public class StorageController : Singleton<StorageController>, IPersistentDataController
    {
        protected Dictionary<string, Memo> memoIdMap = new Dictionary<string, Memo>();
        protected IBaseHologramObjectContainer[] baseHologramObjectContainers;

        public virtual void Save()
        {
            if (!enabled)
            {
                return;
            }
            foreach (var baseHologramObjectContainer in baseHologramObjectContainers)
            {
                Serializator.Serialization(baseHologramObjectContainer.Id, baseHologramObjectContainer.ContainedIds.ToArray());    
            }
            Serializator.Serialization(Data.Instance.AllBaseHologramObjects);
        }

        public virtual void Load()
        {
            baseHologramObjectContainers = GetComponentsInChildren<IBaseHologramObjectContainer>();

            if (baseHologramObjectContainers == null)
            {
                Debug.LogError($"IBaseHologramObjectContainer not found in {name} hierarchy");
                enabled = false;
                return;
            }

            var memos = Serializator.Deserialization<Memo>();
            memoIdMap.Clear();

            if (memos == null)
            {
                return;
            }

            foreach (var memo in memos)
            {
                memoIdMap.Add(memo.Id, memo);
            }

            foreach (var baseHologramContainer in baseHologramObjectContainers)
            {
                var containedIds = Serializator.Deserialization<string[]>(baseHologramContainer.Id);
                if (containedIds == null)
                {
                    continue;
                }

                foreach (var containedId in containedIds)
                {
                    var baseHologramObject = baseHologramContainer.RestoreHologramObjectEvent(memoIdMap[containedId]);
                    Data.Instance.AddHologramObject(baseHologramObject);
                }
            }
        }
    }
}
