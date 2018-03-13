using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoWorld : MonoBehaviour
{

    public Vector2Int gridSize;

    public Vector2 centerPoint;
    public Vector2 areaSize;

    public Vector2 p1;
    public Vector2 p2;

    // Corners of the region, CCW, from bottom-left
    UTMCoords c1;
    UTMCoords c2;
    UTMCoords c3;
    UTMCoords c4;


    public Result[,] geoData;
    float[,] heightMap;

    float xMin, yMin, xMax, yMax;

    // Use this for initialization
    void Start()
    {
        geoData = new Result[gridSize.x, gridSize.y];

        ProcessArea();

        //StartCoroutine(APICommunication.GridElevationRequest(new Rect(xMin, yMin, xMax - xMin, yMax - yMin), gridSize.x));

        Debug.Log(GeoUtils.ConvertToUTM(new GeoCoords(p1.x, p1.y)));

        c1 = GeoUtils.ConvertToUTM(new GeoCoords(p1));
        c2 = GeoUtils.ConvertToUTM(new GeoCoords(p1.x, p2.y));
        c3 = GeoUtils.ConvertToUTM(new GeoCoords(p2));
        c4 = GeoUtils.ConvertToUTM(new GeoCoords(p2.x, p1.y));

        Debug.Log("Distance " + UTMCoords.Distance(c1, c2));

    }


    void ProcessArea()
    {
        if (p1.x <= p2.x)
        {
            xMin = p1.x;
            xMax = p2.x;
        }
        else
        {
            xMin = p2.x;
            xMax = p1.x;
        }

        if (p1.y <= p2.y)
        {
            yMin = p1.y;
            yMax = p2.y;
        }
        else
        {
            yMin = p2.y;
            yMax = p1.y;
        }
    }

    public void onElevationRequestComplete(ElevationResponse response)
    {
        double lowestAltitude = double.MaxValue;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                geoData[x, y] = response.results[y * gridSize.x + x];
                if (geoData[x, y].elevation < lowestAltitude)
                {
                    lowestAltitude = geoData[x, y].elevation;
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

        MeshData meshData = MeshGeneration.GenerateMeshFromHeigthMap(heightMap, .05f);

        GetComponent<MeshFilter>().mesh = meshData.CreateMesh();

        Debug.Log("Populated GeoData");
    }

    
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

public class ElevationResponse
{
    public List<Result> results { get; set; }
    public string status { get; set; }
}