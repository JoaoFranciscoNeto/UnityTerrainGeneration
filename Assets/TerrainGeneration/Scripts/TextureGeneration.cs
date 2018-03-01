using NodeEditorFramework.TextureGenerator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGeneration {
    
    public static HeightMap GenerateHeightMapNoise(NoiseData data, Vector2 texSize)
    {

        float[,] heightMapData = GenerateNoiseData(data, Vector2.zero, texSize);

        HeightMap heightMap = new HeightMap(heightMapData);

        return heightMap;
    }

    public static HeightMap GenerateHeightMapSine(float scale, Vector2 texSize)
    {
        float[,] heightMapData = GenerateSineData(scale, texSize);

        HeightMap heightMap = new HeightMap(heightMapData);

        return heightMap;
    }

    public static HeightMap GenerateHeigthMapWave(float xPeriod, float yPeriod, float turbPower, float turbSize, Vector2 texSize)
    {
        return new HeightMap(GenerateWaveData(xPeriod, yPeriod, turbPower, turbSize, texSize));
    }

    #region Noise
    static float[,] GenerateNoiseData(NoiseData data, Vector2 center, Vector2 noiseSize)
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

        Vector2 rngOffset = new Vector2(prng.Next(-100000, 100000), prng.Next(-100000, 100000));


        if (data.scaleRatio == Vector2.zero)
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

                float sampleX = (float)((x - halfWidth) / data.scale * data.scaleRatio.x) + rngOffset.x;
                float sampleY = (float)((y - halfHeight) / data.scale * data.scaleRatio.y) + rngOffset.y;
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
                    float normalizedHeight = data.curve.Evaluate(noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
    #endregion

    #region Sine

    static float[,] GenerateSineData(float scale, Vector2 noiseSize)
    {
        float[,] noiseMap = new float[(int)noiseSize.x, (int)noiseSize.y];



        for (int y = 0; y < noiseSize.y; y++)
        {
            for (int x = 0; x < noiseSize.x; x++)
            {
                float xyValue = (x * scale + y * scale);
                noiseMap[x, y] = Mathf.Sin(xyValue * Mathf.PI);
            }
        }

        return noiseMap;
    }

    #endregion

    static float[,] GenerateWaveData(float xPeriod, float yPeriod, float turbPower, float turbSize, Vector2 noiseSize)
    {

        float[,] noiseMap = new float[(int)noiseSize.x, (int)noiseSize.y];


        for (int y = 0; y < noiseSize.y; y++)
        {
            for (int x = 0; x < noiseSize.x; x++)
            {
                float xyValue = x * xPeriod / noiseSize.x + y * yPeriod / noiseSize.y + turbPower * turbulence(x, y, turbSize)/256.0f;
                noiseMap[x, y] = Mathf.Abs(DuneWave(xyValue,.72f));
            }
        }
        return noiseMap;

    }

    static float DuneWave(float value, float Xm)
    {
        value = value % 1;

        float ret = 0;

        if (value <= Xm)
        {
            ret = .5f * (1 - Mathf.Cos(Mathf.PI * value / Xm));
        } else {
            ret = (1 - Mathf.Cos((Mathf.PI/2.0f) * ((value - 1)/(Xm-1))));
        }

        return ret;
    }

    static float turbulence(float x, float y, float size)
    {
        float value = 0;
        float initSize = size;

        while (size>=1)
        {
            value += Mathf.PerlinNoise(x / size, y / size) * size;
            size /= 2.0f;
        }

        return (128 * value / initSize);
    }

    public static Texture2D Create2DTexture(float[,] heightMap)
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
