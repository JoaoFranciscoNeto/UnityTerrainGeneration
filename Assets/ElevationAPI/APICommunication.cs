using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICommunication : MonoBehaviour {

    static string elevationPath = "https://maps.googleapis.com/maps/api/elevation/";
    static string apiKey = "AIzaSyAl1Bz8x72uVaARqPtVwF0rvB2STkNI7cc";

    public Vector2 fromCoords;
    public Vector2 toCoords;
    public int samples;

    // Use this for initialization
    void Start ()
    {
        //StartCoroutine(LineElevationRequest(fromCoords, toCoords, samples));


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static ElevationResponse LineElevationRequest(Vector2 from, Vector2 to, int nSamples)
    {
        //https://maps.googleapis.com/maps/api/elevation/json?path=36.578581,-118.291994|36.23998,-116.83171&samples=3&key=AIzaSyAl1Bz8x72uVaARqPtVwF0rvB2STkNI7cc

        string requestPath = elevationPath + "json?path=" + from.x + "," + from.y + "|"+ to.x + "," + to.y + "&samples=" + nSamples + "&key=" + apiKey;

        UnityWebRequest request = UnityWebRequest.Get(requestPath);

        if(request.isNetworkError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            ElevationResponse response = JsonConvert.DeserializeObject<ElevationResponse>(request.downloadHandler.text);

            if(response.status == "OK")
            {
                foreach (Result r in response.results)
                {
                    Debug.Log("(" + r.location.lat + "," + r.location.lng + ") -> " + r.elevation);
                }
            }
            return response;
        }
        

        return null;
        
    }

    public static IEnumerator GridElevationRequest(Rect area, int gridRes)
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

        string requestPath = elevationPath + "json?locations=";

        for (int pIndex = 0; pIndex < samplePoints.Count; pIndex++)
        {
            requestPath += samplePoints[pIndex].x + "," + samplePoints[pIndex].y;
            if (pIndex != samplePoints.Count-1)
            {
                requestPath += "|";
            }
        }

        requestPath += "&key=" + apiKey;

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
            ElevationResponse response = JsonConvert.DeserializeObject<ElevationResponse>(request.downloadHandler.text);

            if (response.status == "OK")
            {
                foreach (Result r in response.results)
                {
                    Debug.Log("(" + r.location.lat + "," + r.location.lng + ") -> " + r.elevation);
                }

                FindObjectOfType<GeoWorld>().onElevationRequestComplete(response);
            }
        }

        yield return null;
    }


    
}
