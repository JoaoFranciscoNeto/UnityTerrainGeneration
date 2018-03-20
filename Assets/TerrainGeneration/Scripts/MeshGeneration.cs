using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGeneration {

    public static MeshData GenerateMeshFromHeigthMap(float[,] heightMap, float heightMultiplier)
    {
        float uvScale = 2f;

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftx = (width - 1) / -2f;
        float topLeftz = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftx + x, heightMap[x, y] * heightMultiplier, topLeftz - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height) * uvScale;

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }

    public static MeshData GenerateGeoMesh(float[,] data)
    {
        int cols = data.GetLength(0);
        int rows = data.GetLength(1);
        

        MeshData meshData = new MeshData(cols, rows);
        int vIndex = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                meshData.vertices[vIndex] = new Vector3((float)x / (cols-1), data[x, y], (float)y / (rows-1));
                meshData.uvs[vIndex] = new Vector2(x / (float)cols, y / (float)rows);

                if (x < cols - 1 && y < rows - 1)
                {
                    int c0 = vIndex;
                    int c1 = vIndex + 1;
                    int c2 = vIndex + cols;
                    int c3 = vIndex + cols + 1;

                    meshData.AddTriangle(c0, c2, c3);
                    meshData.AddTriangle(c3, c1, c0);
                }
                vIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[((meshWidth - 1) * (meshHeight - 1)) * 6];
        uvs = new Vector2[meshWidth * meshHeight];
    }

    public MeshData(Vector3[] vertices, int[] triangles, Vector2[] uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
