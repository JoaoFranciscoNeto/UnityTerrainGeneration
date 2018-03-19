using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICommunication {

    static string googlePath = "https://maps.googleapis.com/maps/api/elevation/";
    static string googleKey = "AIzaSyAl1Bz8x72uVaARqPtVwF0rvB2STkNI7cc";

    static string bingPath = "http://dev.virtualearth.net/REST/v1/Elevation/";
    static string bingKey = "Aij5dZ_8Aewjql1V6eEmd-a2QH87_UdrwS3rQxAiguCX_LdZ4iRFVwnZ5Dxt_RUS";

    public int samples;

    public static IEnumerator GridElevationRequest(Rect area, int gridRes, GeoWorld geoWorld)
    {
        List<Vector2> samplePoints = new List<Vector2>();

        float areaWidth = area.xMax - area.xMin;
        float areaHeight = area.yMax - area.yMin;


        for (float y = area.yMin; y < area.yMax; y+=(areaHeight/gridRes))
        {
            for (float x = area.xMin; x < area.xMax; x += (areaWidth / gridRes))
            {
                //float sampleX = area.xMin + x * areaWidth / gridRes;
                //float sampleY = area.yMin + y * areaHeight / gridRes;
                samplePoints.Add(new Vector2(x,y));
            }
        }

        string requestPath = googlePath + "json?locations=";

        for (int pIndex = 0; pIndex < samplePoints.Count; pIndex++)
        {
            requestPath += samplePoints[pIndex].x + "," + samplePoints[pIndex].y;
            if (pIndex != samplePoints.Count-1)
            {
                requestPath += "|";
            }
        }

        requestPath += "&key=" + googleKey;

        Debug.Log(requestPath);

        UnityWebRequest request = UnityWebRequest.Get(requestPath);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            GoogleElevationResponse response = JsonConvert.DeserializeObject<GoogleElevationResponse>(request.downloadHandler.text);

            if (response.status == "OK")
            {
                foreach (Result r in response.results)
                {
                    Debug.Log("(" + r.location.lat + "," + r.location.lng + ") -> " + r.elevation);
                }


            }
        }

        yield return null;
    }

    public static IEnumerator BingElevationRequest(GeoArea area, int gridRes, GeoChunk geoChunk)
    {
        GeoCoords sw = GeoUtils.ConvertToGeo(area.sw);
        GeoCoords ne = GeoUtils.ConvertToGeo(area.ne);

        string requestPath = bingPath + "Bounds?bounds=" + sw.latitude + "," + sw.longitude + "," + ne.latitude + "," + ne.longitude + "&rows=" + gridRes + "&cols=" + gridRes;


        requestPath += "&key=" + bingKey;



        UnityWebRequest request = UnityWebRequest.Get(requestPath);
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {

            //GoogleElevationResponse response = JsonConvert.DeserializeObject<GoogleElevationResponse>(request.downloadHandler.text);
            BingElevationResponse response = JsonConvert.DeserializeObject<BingElevationResponse>(request.downloadHandler.text);
            geoChunk.onDataReceived(response);
        }

        yield return null;
    }
}
