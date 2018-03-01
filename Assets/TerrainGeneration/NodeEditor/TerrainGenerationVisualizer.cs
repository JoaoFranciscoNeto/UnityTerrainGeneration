using NodeEditorFramework.TextureGenerator;
using NodeEditorFramework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.TerrainGenerator
{

    [Node(false, "Terrain/Visualizer")]
    public class TerrainGenerationVisualizer : Node
    {
        public const string ID = "terrainInfo";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Terrain Info"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        [ValueConnectionKnob("Terrain", Direction.In, "Terrain")]
        public ValueConnectionKnob terrainKnob;

        public Texture2D tex;

        public int texSize = 32;
        TerrainFunc terrainFunc;

        public override void NodeGUI()
        {
            terrainKnob.DisplayLayout();

            texSize = RTEditorGUI.IntField("Size", texSize);

            if (tex != null)
            {
                RTTextureViz.DrawTexture(tex, 64);
                GUILayout.Label("Size: " + tex.width + "x" + tex.height + "");
                GUILayout.Label("Format: " + tex.format);
            }

            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }
        }

        public override bool Calculate()
        {
            terrainFunc = terrainKnob.connected() ? terrainKnob.GetValue<TerrainFunc>() : null;

            tex = Create2DTexture(GenerateData());

            return true;
        }

        float[,] GenerateData()
        {

            float[,] data = new float[texSize, texSize];
            for (int y = 0; y < texSize; y++)
            {
                for (int x = 0; x < texSize; x++)
                {
                    data[x, y] = terrainFunc.generateFunc(x, y);
                }
            }

            return data;
        }

        Texture2D Create2DTexture(float[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            Color[] colorMap = new Color[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, data[x, y]);
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
}
