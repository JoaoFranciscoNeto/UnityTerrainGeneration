using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoChunk : MonoBehaviour {

    public GeoArea chunkArea;
    
    MeshFilter meshFilter;

    int res = 4;
    
    public void Create(GeoArea chunkArea)
    {
        meshFilter = GetComponent<MeshFilter>();

        transform.position = new Vector3(chunkArea.center.x,0,chunkArea.center.y) + GeoWorld.offsetToCenter;
        transform.localScale = new Vector3((float)chunkArea.width, 1, (float)chunkArea.length);

        Debug.Log(chunkArea);

        this.chunkArea = chunkArea;
        StartCoroutine(APICommunication.BingElevationRequest(chunkArea, res, this));
    }

    public void onDataReceived(BingElevationResponse response)
    {
        float[,] heightMap = new float[res, res];

        for (int y = 0; y < res; y++)
        {
            for (int x = 0; x < res; x++)
            {
                heightMap[x, y] = response.resourceSets[0].resources[0].elevations[y * res + x];
            }
        }

        MeshData data = MeshGeneration.GenerateGeoMesh(heightMap);

        meshFilter.mesh = data.CreateMesh();



    }
}
