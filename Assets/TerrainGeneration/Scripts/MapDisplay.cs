using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public static Vector2 displaySize = new Vector2(256,256);

    public Renderer planeRenderer;
    public MeshFilter meshFilter;
    public GameObject meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        planeRenderer.sharedMaterial.mainTexture = texture;
        planeRenderer.transform.localScale = new Vector3(texture.width/10f, 1, texture.height/10f);
    }

    public void DrawMesh(MeshData data)
    {
        Mesh m = data.CreateMesh();
        meshRenderer.GetComponent<MeshFilter>().sharedMesh = m;
        meshRenderer.GetComponent<MeshCollider>().sharedMesh = m;
    }
}
