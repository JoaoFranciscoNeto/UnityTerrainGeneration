using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APICommunication : MonoBehaviour {
    

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(GetText());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://maps.googleapis.com/maps/api/elevation/json?locations=40.714728,-73.998672|-34.397,150.644&key=AIzaSyAl1Bz8x72uVaARqPtVwF0rvB2STkNI7cc");
        

        yield return www.Send();

        if (www.isError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}
