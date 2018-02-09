using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class World : MonoBehaviour {

    public enum DrawMode
    {
        NoiseMap,
        Mesh
    }

    public DrawMode drawMode;
    public bool displayAutoUpdate;
    
    public NoiseData[] noiseData;

    NoiseGeneration noiseGen;

    public float heightMultiplier;

    // ChunkProperties
    public const int mapChunkSize = 241;
    public Material chunkMaterial;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private void Start()
    {
        noiseGen = GetComponent<NoiseGeneration>();

        noiseGen.SetNoiseSize(Vector2.one * mapChunkSize);
        NoiseGeneration.normalizeMode = NoiseGeneration.NormalizeMode.Global;
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    public void DisplayMap()
    {
        noiseGen = GetComponent<NoiseGeneration>();

        noiseGen.SetNoiseSize(Vector2.one * mapChunkSize);
        NoiseGeneration.normalizeMode = NoiseGeneration.NormalizeMode.Global;

        MapData mapData = GenerateMapData();
        MapDisplay display = FindObjectOfType<MapDisplay>();

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                display.DrawTexture(noiseGen.Get2DTexture());
                break;   
            case DrawMode.Mesh:
                display.DrawMesh(MeshGeneration.GenerateMeshFromHeigthMap(mapData.heightMap,heightMultiplier));
                break;
        }

    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, callback);
        };
        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, Action<MeshData> callback)
    {
        MeshData meshData = MeshGeneration.GenerateMeshFromHeigthMap(mapData.heightMap, heightMultiplier);
        lock (meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    MapData GenerateMapData()
    {
        return GenerateMapData(Vector2.zero);
    }

    MapData GenerateMapData(Vector2 center)
    {
        // Generate texture based on display size
        if (noiseGen == null)
            noiseGen = GetComponent<NoiseGeneration>();

        noiseGen.SetNoiseData(noiseData);

        float[,] heightMap = noiseGen.CombinedNoiseMap(center);
        return new MapData(heightMap);
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}

public struct MapData
{
    public readonly float[,] heightMap;

    public MapData(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
}