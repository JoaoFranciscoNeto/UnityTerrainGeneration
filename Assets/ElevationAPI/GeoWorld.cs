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


    public Result[,] geoData;
    float[,] heightMap;

    float xMin, yMin, xMax, yMax;

    // Use this for initialization
    void Start()
    {
        geoData = new Result[gridSize.x, gridSize.y];

        ProcessArea();

        StartCoroutine(APICommunication.GridElevationRequest(new Rect(xMin, yMin, xMax - xMin, yMax - yMin), gridSize.x));

        int zone = GetZone(p1.x, p1.y);
        string band = GetBand(p1.x);
        //Transform to UTM
        CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
        ICoordinateSystem wgs84geo = ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84;
        ICoordinateSystem utm = ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WGS84_UTM(zone, p1.x > 0);
        ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(wgs84geo, utm);
        double[] pUtm = trans.MathTransform.Transform(new double[] { p1.y, p1.x });

        double easting = pUtm[0];
        double northing = pUtm[1];

        Debug.Log(zone + " " + band + " " + easting + " " + northing);
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

    private static int GetZone(double latitude, double longitude)
    {
        // Norway
        if (latitude >= 56 && latitude < 64 && longitude >= 3 && longitude < 13)
            return 32;

        // Spitsbergen
        if (latitude >= 72 && latitude < 84)
        {
            if (longitude >= 0 && longitude < 9)
                return 31;
            else if (longitude >= 9 && longitude < 21)
                return 33;
            if (longitude >= 21 && longitude < 33)
                return 35;
            if (longitude >= 33 && longitude < 42)
                return 37;
        }

        return (int)Mathf.Ceil(((float)longitude + 180f) / 6f);
    }
    private static string GetBand(double latitude)
    {
        if (latitude <= 84 && latitude >= 72)
            return "X";
        else if (latitude < 72 && latitude >= 64)
            return "W";
        else if (latitude < 64 && latitude >= 56)
            return "V";
        else if (latitude < 56 && latitude >= 48)
            return "U";
        else if (latitude < 48 && latitude >= 40)
            return "T";
        else if (latitude < 40 && latitude >= 32)
            return "S";
        else if (latitude < 32 && latitude >= 24)
            return "R";
        else if (latitude < 24 && latitude >= 16)
            return "Q";
        else if (latitude < 16 && latitude >= 8)
            return "P";
        else if (latitude < 8 && latitude >= 0)
            return "N";
        else if (latitude < 0 && latitude >= -8)
            return "M";
        else if (latitude < -8 && latitude >= -16)
            return "L";
        else if (latitude < -16 && latitude >= -24)
            return "K";
        else if (latitude < -24 && latitude >= -32)
            return "J";
        else if (latitude < -32 && latitude >= -40)
            return "H";
        else if (latitude < -40 && latitude >= -48)
            return "G";
        else if (latitude < -48 && latitude >= -56)
            return "F";
        else if (latitude < -56 && latitude >= -64)
            return "E";
        else if (latitude < -64 && latitude >= -72)
            return "D";
        else if (latitude < -72 && latitude >= -80)
            return "C";
        else
            return null;
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