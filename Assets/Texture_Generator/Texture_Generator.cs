﻿using System;
using UnityEngine;

namespace NodeEditorFramework.TextureGenerator
{
	public class Texture2DType : ValueConnectionType
	{
		public override string Identifier { get { return "Texture"; } }
		public override Type Type { get { return typeof(Texture2D); } }
		public override Color Color { get { return Color.green; } }
	}

	public class ChannelType : ValueConnectionType
	{
		public override string Identifier { get { return "Channel"; } }
		public override Type Type { get { return typeof(Channel); } }
		public override Color Color { get { return Color.yellow; } }
	}

    public class HeightMapType : ValueConnectionType
    {
        public override string Identifier { get { return "HeightMap"; } }
        public override Type Type { get { return typeof(HeightMap); } }
        public override Color Color { get { return Color.magenta; } }

    }

    public class HeightMap
    {
        public float[,] heightMap;
        public Texture2D texture;

        

        public HeightMap(float[,] heightMap)
        {
            this.heightMap = heightMap;
            this.texture = TextureGeneration.Create2DTexture(heightMap);
        }

        public HeightMap(float[,] heightMap, Texture2D texture)
        {
            this.heightMap = heightMap;
            this.texture = texture;
        }

    }

	public class Channel
	{
		public string name;
		public float[,] data;

		public Channel(string channelName, float[,] channelData)
		{
			name = channelName;
			data = channelData;
		}

		public float GetValue(int x, int y, int totalWidth, int totalHeight)
		{
			return data == null ? 0 :
					data[Mathf.FloorToInt((float)x / totalWidth * width),
						 Mathf.FloorToInt((float)y / totalHeight * height)];
		}

		public int width
		{
			get { return data == null ? 1 : data.GetLength(0); }
		}

		public int height
		{
			get { return data == null ? 1 : data.GetLength(1); }
		}
	}
}