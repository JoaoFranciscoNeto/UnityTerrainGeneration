using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    World world;

    GameObject meshObject;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    Bounds bounds;

    Vector2 position;

    public Chunk(Vector2 coord, int size, Transform parent, World world)
    {
        this.world = world;

        position = coord * size;
        bounds = new Bounds(position, Vector2.one * size);
        Vector3 pos3 = new Vector3(position.x, 0, position.y);

        meshObject = new GameObject("Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.sharedMaterial = world.chunkMaterial;
        meshObject.transform.parent = parent;
        meshObject.transform.position = pos3;

        SetVisible(false);

        world.RequestMapData(position, OnMapDataReceived);
    }

    void OnMapDataReceived(MapData mapData)
    {
        world.RequestMeshData(mapData, OnMeshDataReceived);
    }

    void OnMeshDataReceived(MeshData meshData)
    {
        meshFilter.mesh = meshData.CreateMesh();
    }


    public void UpdateChunk()
    {
        SetVisible(Mathf.Sqrt(bounds.SqrDistance(EndlessTerrain.viewerPosition)) <= EndlessTerrain.maxViewDist);
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }
}
