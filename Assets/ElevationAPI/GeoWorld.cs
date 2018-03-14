using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoWorld : MonoBehaviour
{

    public Vector2Int gridSize;
    
    public Vector2 p1;
    public Vector2 p2;

    public GeoArea area;
    public List<GeoArea> chunksAreas;


    Vector3 offsetToCenter;

    // Use this for initialization
    void Start()
    {

        Debug.Log(GeoUtils.ConvertToUTM(new GeoCoords(p1.x, p1.y)));

        area = new GeoArea(GeoUtils.ConvertToUTM(new GeoCoords(p1.x, p1.y)), GeoUtils.ConvertToUTM(new GeoCoords(p2.x, p2.y)));

        Debug.Log(area);

        chunksAreas = area.SubdivideArea(3, 4);

        foreach (GeoArea area in chunksAreas)
        {
            Debug.Log(area);
        }

        offsetToCenter = new Vector3(-((float)area.sw.easting + (float)area.width / 2f), 0, -((float)area.sw.northing + (float)area.length / 2f));
    }

    private void OnDrawGizmos()
    {
        if (chunksAreas == null)
            return;


        foreach (GeoArea area in chunksAreas)
        {
            Gizmos.DrawWireCube(
                new Vector3((float)area.sw.easting + (float)area.width / 2f, 0, (float)area.sw.northing + (float)area.length / 2f) + offsetToCenter,
                new Vector3((float)area.width, 1, (float)area.length)                
                );
        }
    }
    /*
    public void onElevationRequestComplete(GoogleElevationResponse response)
    {
        double lowestAltitude = double.MaxValue;
        double heighestAltitude = double.MinValue;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                geoData[x, y] = response.results[y * gridSize.x + x];
                if (geoData[x, y].elevation < lowestAltitude)
                {
                    lowestAltitude = geoData[x, y].elevation;
                }
                else if (geoData[x, y].elevation > heighestAltitude)
                {
                    heighestAltitude = geoData[x, y].elevation;
                }
            }
        }


        heightMap = new float[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                heightMap[x, y] = (float)(geoData[x, y].elevation - lowestAltitude);
            }
        }

        MeshData meshData = MeshGeneration.GenerateMeshFromHeigthMap(heightMap, 1);

        GetComponent<MeshFilter>().mesh = meshData.CreateMesh();

        Debug.Log("Populated GeoData");

        zoneSize.y = (float)(heighestAltitude - lowestAltitude);
    }*/



}



public class Location
{
    public double lat { get; set; }
    public double lng { get; set; }
}

public class Result
{
    public double elevation { get; set; }
    public Location location { get; set; }
    public double resolution { get; set; }
}

public class GoogleElevationResponse
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}

public class Resource
{
    public string __type { get; set; }
    public List<int> elevations { get; set; }
    public int zoomLevel { get; set; }
}

public class ResourceSet
{
    public int estimatedTotal { get; set; }
    public List<Resource> resources { get; set; }
}

public class BingElevationResponse
{
    public string authenticationResultCode { get; set; }
    public string brandLogoUri { get; set; }
    public string copyright { get; set; }
    public List<ResourceSet> resourceSets { get; set; }
    public int statusCode { get; set; }
    public string statusDescription { get; set; }
    public string traceId { get; set; }
}