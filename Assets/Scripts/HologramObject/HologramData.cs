using System;
using Storage;
using UnityEngine;

public struct HologramData
{
    public string Id { get; }
    public string PrefabName { get; }
    public string HologramName { get; }
    public SpatialData SpatialData { get; }
    public RendererData RendererData { get; }

    public HologramData(Memo memo)
    {
        Id = memo.Id;
        PrefabName = memo.Name;
        HologramName = memo.HologramName;
        SpatialData = new SpatialData(
            new Vector3(memo.PosX, memo.PosY, memo.PosZ),
            Quaternion.Euler(memo.RotX, memo.RotY, memo.RotZ),
            new Vector3(memo.ScaleX, memo.ScaleY, memo.ScaleZ));
        RendererData = new RendererData(null,
            new Color(memo.ColorR, memo.ColorG, memo.ColorB, memo.ColorA));
    }

    public HologramData(string prefabName, string id = null, string hologramName = "NoName", SpatialData spatialData = default, RendererData rendererData = default)
    {
        Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
        PrefabName = prefabName;
        HologramName = hologramName;
        SpatialData = spatialData;
        RendererData = rendererData;
    }
}

public readonly struct SpatialData
{
    public Vector3 Position { get; }
    public Quaternion Rotation { get; }
    public Vector3 Scale { get; }

    public SpatialData(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }
}

public struct RendererData
{
    private readonly Renderer renderer;
    public Color Color
    {
        get
        {
            if (renderer == null || renderer.material == null)
            {
                return color;
            }

            return renderer.material.color;
        }
        set
        {
            color = value;
            renderer.material.color = color;
        } 
    }

    private Color color;

    public RendererData(Renderer renderer, Color color)
    {
        this.renderer = renderer;
        if (this.renderer != null && this.renderer.material != null)
        {
            renderer.material.color = color;
        }
        this.color = color;
    }
}