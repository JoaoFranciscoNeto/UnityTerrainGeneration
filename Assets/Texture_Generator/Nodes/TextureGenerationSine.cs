using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/Texture Generation/Sine")]
    public class TextureGenerationSin : Node
    {
        public const string ID = "texSinGen";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }

        public override string Title { get { return "Sine Generation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }

        [ValueConnectionKnob("Texture", Direction.Out, "Texture")]
        public ValueConnectionKnob textureOutput;
        /*
        [ValueConnectionKnob("HeightMap", Direction.Out, "HeightMap")]
        public ValueConnectionKnob heightMapOutput;
        */
        public Texture2D tex;

        public float scale;
        public Vector2 scaleRatio;
        public Vector2 textureSize = new Vector2(256, 256);


        public override void NodeGUI()
        {

            textureOutput.DisplayLayout();
            //heightMapOutput.DisplayLayout();

            scale = RTEditorGUI.Slider("Texture Scale", scale, 1, 100);
            scaleRatio = RTEditorGUI.Vector2Field("Scale Ratio", scaleRatio);
            textureSize = RTEditorGUI.Vector2Field("Texture Size", textureSize);

            if (GUI.changed)
            { // Texture has been changed
                tex = TextureGeneration.Generate2DTexture(new NoiseData(scale, scaleRatio), textureSize);

                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }
        }

        public override bool Calculate()
        {
            textureOutput.SetValue(tex);
            //heightMapOutput.SetValue(TextureGeneration.GenerateNoiseMap(new NoiseData(scale, scaleRatio), Vector2.zero, textureSize));
            return true;
        }
    }
}
