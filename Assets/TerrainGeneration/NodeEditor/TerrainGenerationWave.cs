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

        public enum WaveType { Sine, SandDome, SandDune, Triangle, SeeSaw }
        public WaveType type;

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

            type = (WaveType)RTEditorGUI.EnumPopup(new GUIContent("Wave Type", "The type of wave to be generated"), type);

            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {
            terrainKnob.SetValue(new WaveTexture(type, angle, scale));

            return true;
        }

    }

    public class WaveTexture : TerrainFunc
    {
        public float angle;
        public float scale;
        public TerrainGenerationWave.WaveType type;

        public WaveTexture(TerrainGenerationWave.WaveType type, float angle, float scale)
        {
            this.type = type;
            this.angle = angle;
            this.scale = scale;

            generateFunc = WaveGen;
        }

        float WaveGen(float x, float y)
        {
            float xyValue = (x * (Mathf.Cos(angle * Mathf.Deg2Rad) * scale) + y * (Mathf.Sin(angle * Mathf.Deg2Rad) * scale));
            float val = 0;

            switch (type)
            {
                case TerrainGenerationWave.WaveType.Sine:
                    val = Mathf.Sin(xyValue * Mathf.PI);
                    break;
                case TerrainGenerationWave.WaveType.SeeSaw:
                    val = xyValue % 1;
                    break;
            }

            return val;
        }
    }
}