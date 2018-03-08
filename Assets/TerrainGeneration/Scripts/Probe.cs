using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Probe : MonoBehaviour {

    Vector3 probePoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        probePoint = TerrainManager.surfacePoint(new Vector2(transform.position.x, transform.position.z));

	}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, probePoint);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(probePoint, 2f);
    }


}
