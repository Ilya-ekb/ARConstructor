using System;
using UnityEngine;

namespace Storage
{
    [Serializable]
    public class Memo 
    {
        public string Id { get; }
        public string Name { get; }
        public float PosX { get; }
        public float PosY { get; }
        public float PosZ { get; }
        public float RotX { get; }
        public float RotY { get; }
        public float RotZ { get; }
        public float ScaleX { get; }
        public float ScaleY { get; }
        public float ScaleZ { get; }
        public float ColorR { get; }
        public float ColorG { get; }
        public float ColorB { get; }
        public float ColorA { get; }

        public Memo(IBaseHologramObject baseObject)
        {
            var spatialData = new SpatialData(
                baseObject.GameObject.transform.position,
                baseObject.GameObject.transform.rotation, 
                baseObject.GameObject.transform.localScale);

            baseObject.UpdateObject(
                new HologramData(
                    baseObject.HologramData.PrefabName, 
                    baseObject.HologramData.Id,
                    spatialData, 
                    baseObject.HologramData.RendererData));

            Id = baseObject.HologramData.Id;
            Name = baseObject.HologramData.PrefabName;
           
            PosX = baseObject.HologramData.SpatialData.Position.x;
            PosY = baseObject.HologramData.SpatialData.Position.y;
            PosZ = baseObject.HologramData.SpatialData.Position.z;

            RotX = baseObject.HologramData.SpatialData.Rotation.x;
            RotY = baseObject.HologramData.SpatialData.Rotation.y;
            RotZ = baseObject.HologramData.SpatialData.Rotation.z;

            ScaleX = baseObject.HologramData.SpatialData.Scale.x;
            ScaleY = baseObject.HologramData.SpatialData.Scale.y;
            ScaleZ = baseObject.HologramData.SpatialData.Scale.z;

            ColorR = baseObject.HologramData.RendererData.Color.r;
            ColorG = baseObject.HologramData.RendererData.Color.g;
            ColorB = baseObject.HologramData.RendererData.Color.b;
            ColorA = baseObject.HologramData.RendererData.Color.a;
        }
    }
}
