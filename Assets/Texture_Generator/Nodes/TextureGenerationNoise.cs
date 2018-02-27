using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/Texture Generation/Noise")]
    public class TextureGenerationNoise : Node
    {
        public const string ID = "texNoiseGen";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }

        public override string Title { get { return "Noise Generation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }

        [ValueConnectionKnob("HeightMap", Direction.Out, "HeightMap")]
        public ValueConnectionKnob heightMapType;

        public HeightMap HeightMap;

        Texture2D tex;

        public float scale;
        public Vector2 scaleRatio;
        public Vector2 textureSize = new Vector2(256, 256);


        public override void NodeGUI()
        {

            heightMapType.DisplayLayout();


            scale = RTEditorGUI.Slider("Texture Scale", scale, 1, 1000);
            scaleRatio = RTEditorGUI.Vector2Field("Scale Ratio", scaleRatio);
            textureSize = RTEditorGUI.Vector2Field("Texture Size", textureSize);

            if (GUI.changed)
            { // Texture has been changed

                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }
        }

        public override bool Calculate()
        {
            HeightMap = TextureGeneration.GenerateHeightMapNoise(new NoiseData(scale, scaleRatio), textureSize);
            heightMapType.SetValue(HeightMap);

            return true;
        }
    }
}
