using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoChunk : MonoBehaviour {

    public GeoArea chunkArea;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    
    public void Create(GeoArea chunkArea)
    {
        this.chunkArea = chunkArea;
        StartCoroutine(APICommunication.BingElevationRequest(chunkArea, 4, this));
    }

    public void onDataReceived(BingElevationResponse response)
    {
        Debug.Log(response.resourceSets[0].resources[0].elevations);
    }
}
