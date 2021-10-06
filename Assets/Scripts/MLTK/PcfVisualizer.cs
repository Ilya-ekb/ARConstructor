using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MLTK
{
    public class PcfVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject visualPrefab;
        private Dictionary<string, GameObject> visualGameObjectsMap = new Dictionary<string, GameObject>();
        private void Start()
        {
#if PLATFORM_LUMIN
            var result = MLPersistentCoordinateFrames.Start();
            if (!result.IsOk)
            {
                Debug.LogError("Error: Failed starting MLPersistentCoordinateFrames, disabling script. Reason:" + result);
                enabled = false;
                return;
            }

            MLPersistentCoordinateFrames.OnLocalized += HandleOnLocalized;

            if (MLPersistentCoordinateFrames.IsLocalized)
            {
                DisplayAllPCFs();
            }
#endif
        }

        private void HandleOnLocalized(bool localized)
        {
            if (localized)
            {
                DisplayAllPCFs();
            }
        }
#if PLATFORM_LUMIN
        private void DisplayAllPCFs(Action<string, GameObject> newPcfAddedAction = null)
        {
            var allPCFs = AllPcFs(out var result);

            if (allPCFs == null)
            {
                return;
            }

            foreach (var pcf in allPCFs)
            {
                var id = pcf.CFUID.ToString();
                if (visualGameObjectsMap.ContainsKey(id))
                {
                    visualGameObjectsMap[id].transform.position = pcf.Position;
                    visualGameObjectsMap[id].transform.rotation = pcf.Rotation;

                }
                else
                {
                    var newPCFObj = visualPrefab == null ? null : Instantiate(visualPrefab, pcf.Position, pcf.Rotation, transform);
                    newPCFObj.SetActive(false);
                    
                    visualGameObjectsMap.Add(id, newPCFObj);
                    if (newPcfAddedAction == null)
                    {
                        newPCFObj.SetActive(true);
                    }
                    else
                    {
                        newPcfAddedAction.Invoke(id, newPCFObj);
                    }
                }
            }
        }

        private static List<MLPersistentCoordinateFrames.PCF> AllPcFs(out MLResult result)
        {
            result = MLPersistentCoordinateFrames.FindAllPCFs(out var allPCFs);
            if (!result.IsOk)
            {
                if (result.Result == MLResult.Code.PassableWorldLowMapQuality ||
                    result.Result == MLResult.Code.PassableWorldUnableToLocalize)
                {
                    Debug.LogWarningFormat("Map quality not sufficient enough for PCFVisualizer to find all pcfs. Reason: {0}",
                        result);
                }
                else
                {
                    Debug.LogErrorFormat(
                        "Error: PCFVisualizer failed to find all PCFs because MLPersistentCoordinateFrames failed to get all PCFs. Reason: {0}",
                        result);
                }
            }

            return allPCFs;
        }
#endif
    }

}
