using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TerrainGenerator
{
    [Node(false, "Terrain/Generator/Noise")]
    public class TerrainGenerationNoise : Node
    {
        public const string ID = "terrainGeneratorNoise";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }
        public override string Title { get { return "Terrain Wave Generation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }
        

        [ValueConnectionKnob("Terrain", Direction.Out, "Terrain")]
        public ValueConnectionKnob terrainKnob;

        public float scale;
        public int seed;

        public override void NodeGUI()
        {
            terrainKnob.DisplayLayout();


            seed = RTEditorGUI.IntField("Seed", seed);
            scale = RTEditorGUI.Slider("Scale", scale, 0, 1000);

            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {
            terrainKnob.SetValue(new NoiseTerrain(seed, scale));

            return true;
        }

    }

    public class NoiseTerrain : TerrainFunc
    {
        public float scale;
        public int seed;
        System.Random prng;
        Vector2 rngOffset;
        public NoiseTerrain(int seed, float scale)
        {
            this.seed = seed;
            this.scale = scale;

            if (scale == 0)
                this.scale = .001f;
            
            if (seed != -1)
            {
                prng = new System.Random(seed);
            }
            else
            {
                prng = new System.Random();
            }

            rngOffset = new Vector2(prng.Next(-100000, 100000), prng.Next(-100000, 100000));

            generateFunc = NoiseGen;
        }

        float NoiseGen(float x, float y)
        {


            float sampleX = (float)((x) / scale) + rngOffset.x;
            float sampleY = (float)((y) / scale) + rngOffset.y;



            return Mathf.PerlinNoise(sampleX,sampleY);
        }
    }
}