using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;


namespace NodeEditorFramework.TerrainGenerator
{
    [Node(false, "Terrain/Generator/Wave")]
    public class TerrainGenerationWave : Node
    {
        public const string ID = "terrainGeneratorWave";
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

        public WaveTexture WaveTexture;
        
        public float angle;
        public float scale;

        public override void NodeGUI()
        {
            terrainKnob.DisplayLayout();
            angle = RTEditorGUI.Slider("Angle", angle, 0, 360);
            scale = RTEditorGUI.Slider("Scale", scale, 0, 1);

            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {
            terrainKnob.SetValue(new WaveTexture(angle,scale));

            return true;
        }

    }

    public class WaveTexture : TerrainFunc
    {
        public float angle;
        public float scale;

        public WaveTexture(float angle, float scale)
        {
            this.angle = angle;
            this.scale = scale;

            generateFunc = WaveGen;
        }

        float WaveGen(float x, float y)
        {
            float xyValue = (x * (Mathf.Cos(angle * Mathf.Deg2Rad) * scale) + y * (Mathf.Sin(angle * Mathf.Deg2Rad) * scale));
            return Mathf.Sin(xyValue * Mathf.PI);
        }
    }
}