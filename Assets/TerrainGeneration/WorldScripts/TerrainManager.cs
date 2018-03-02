using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class TerrainManager : MonoBehaviour {


    Queue<TerrainThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<TerrainThreadInfo<MeshData>>();


    void Start () {
		
	}
	
	void Update () {
		
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
