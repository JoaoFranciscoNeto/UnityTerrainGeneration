using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using NodeEditorFramework.TerrainGenerator;

public class TerrainManager : MonoBehaviour {


    public Vector2 worldSize = new Vector2(128,128);

    TerrainFunc _terrainFunc;

    public TerrainFunc terrainFunction
    {
        get { return _terrainFunc; }
        set
        {
            _terrainFunc = value;
            GenerateData();
        }
    }

    float[,] heightMap;
    Mesh terrainMesh;

    Queue<TerrainThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<TerrainThreadInfo<MeshData>>();

    void Start () {
        if(Application.isPlaying)
            GetComponent<MeshCollider>().sharedMesh = terrainMesh;
    }

    void Update () {

    }

    void GenerateData()
    {
        heightMap = new float[(int)worldSize.x, (int)worldSize.y];
        for (int y = 0; y < worldSize.y; y++)
        {
            for (int x = 0; x < worldSize.x; x++)
            {
                heightMap[x, y] = _terrainFunc.generateFunc(x, y) * 10;
            }
        }

        terrainMesh = MeshGeneration.GenerateMeshFromHeigthMap(heightMap, 1).CreateMesh();
        GetComponent<MeshFilter>().mesh = terrainMesh;

    }

    public static Vector3 surfacePoint(Vector2 point)
    {
        TerrainManager terrainManager = FindObjectOfType<TerrainManager>();
        int x = Mathf.FloorToInt(point.x) + (int) (terrainManager.worldSize.x / 2);
        int y = Mathf.FloorToInt(point.y) + (int) (terrainManager.worldSize.y / 2);
        float height = terrainManager.heightMap[x, y];

        return new Vector3(point.x,height,point.y);
        
    }


    struct TerrainThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public TerrainThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}
