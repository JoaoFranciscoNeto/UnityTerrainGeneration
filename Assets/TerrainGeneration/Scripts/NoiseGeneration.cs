using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGeneration : MonoBehaviour {

    public enum NormalizeMode { Local, Global }

    public static NormalizeMode normalizeMode;

    public Vector2 noiseSize;
    NoiseData[] noiseData;

    public void SetNoiseData(NoiseData[] noiseData)
    {
        this.noiseData = noiseData;
    }
    public void SetNoiseSize(Vector2 noiseSize)
    {
        this.noiseSize = noiseSize;
    }

    public Texture2D Get2DTexture()
    {
        return Create2DTexture(CombinedNoiseMap(Vector2.zero));
    }

    public float[,] CombinedNoiseMap(Vector2 center)
    {
        List<float[,]> noiseMapList = new List<float[,]>();

        foreach(NoiseData data in noiseData)
        {
            noiseMapList.Add(GenerateNoiseMap(data,center));
        }

        float[,] combinedNoiseMap = new float[(int)noiseSize.x, (int)noiseSize.y];

        int mapCount = noiseData.Length;

        for (int y = 0; y < noiseSize.y; y++)
        {
            for (int x = 0; x < noiseSize.x; x++)
            {
                for (int i = 0; i < mapCount; i++)
                {
                    combinedNoiseMap[x, y] += noiseMapList[i][x, y];
                }
                combinedNoiseMap[x, y] /= mapCount;
            }
        }

        return combinedNoiseMap;
    }

    float[,] GenerateNoiseMap(NoiseData data, Vector2 center)
    {
        float[,] noiseMap = new float[(int)noiseSize.x, (int)noiseSize.y];

        System.Random prng;
        if (data.seed != -1)
        {
            prng = new System.Random(data.seed);
        }
        else
        {
            prng = new System.Random();
        }

        float offsetX = prng.Next(-100000, 100000) + center.x + data.offset.x;
        float offsetY = prng.Next(-100000, 100000) + center.y + data.offset.y;
        
        
        if (data.scaleRatio == Vector2.zero )
        {
            data.scaleRatio = Vector2.one;
        }

        if (data.scale <= 0)
        {
            data.scale = 1f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = noiseSize.x / 2f;
        float halfHeight = noiseSize.y / 2f;


        for (int y = 0; y < noiseSize.y; y++)
        {
            for (int x = 0; x < noiseSize.x; x++)
            {

                float sampleX = (float)((x - halfWidth + offsetX) / data.scale * data.scaleRatio.x);
                float sampleY = (float)((y - halfHeight - offsetY) / data.scale * data.scaleRatio.y);
                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY);
                
                if (perlinValue > maxNoiseHeight)
                    maxNoiseHeight = perlinValue;

                if (perlinValue < minNoiseHeight)
                    minNoiseHeight = perlinValue;

                noiseMap[x, y] = perlinValue;
            }
        }
        
        for (int y = 0; y < noiseSize.y; y++)
        {
            for (int x = 0; x < noiseSize.x; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = data.curve.Evaluate(Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]));
                } else
                {
                    float normalizedHeight = data.curve.Evaluate(noiseMap[x, y]);
                }
            }
        }

        return noiseMap;
    }

    Texture2D Create2DTexture(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

}

[System.Serializable]
public class NoiseData
{
    public int seed;
    public float scale;
    public Vector2 scaleRatio;
    public Vector2 offset;
    public AnimationCurve curve;


}
