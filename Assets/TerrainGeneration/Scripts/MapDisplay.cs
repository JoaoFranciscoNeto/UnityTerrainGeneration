using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public static Vector2 displaySize = new Vector2(256,256);

    public Renderer planeRenderer;
    public MeshFilter meshFilter;

    public void DrawTexture(Texture2D texture)
    {
        planeRenderer.sharedMaterial.mainTexture = texture;
        planeRenderer.transform.localScale = new Vector3(texture.width/10f, 1, texture.height/10f);
    }

    public void DrawMesh(MeshData data)
    {
        meshFilter.sharedMesh = data.CreateMesh();
    }
}
