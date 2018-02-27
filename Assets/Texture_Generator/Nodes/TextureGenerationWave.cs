using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/Texture Generation/Wave")]
    public class TextureGenerationWave : Node
    {
        public const string ID = "texWaveGen";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }

        public override string Title { get { return "Wave Generation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }

        [ValueConnectionKnob("HeightMap", Direction.Out, "HeightMap")]
        public ValueConnectionKnob heightMapType;

        public HeightMap HeightMap;

        Texture2D tex;
        
        public float xPeriod;
        public float yPeriod;
        public float turbPower;
        public float turbSize;
        public Vector2 textureSize = new Vector2(256, 256);

        public override void NodeGUI()
        {

            heightMapType.DisplayLayout();
            
            xPeriod = RTEditorGUI.Slider("X Period", xPeriod, 0, 100);
            yPeriod = RTEditorGUI.Slider("Y Period", yPeriod, 0, 100);
            turbPower = RTEditorGUI.Slider("Turbulence Power", turbPower, 0, 3);
            turbSize = RTEditorGUI.Slider("Turbulence Size", turbSize, 0, 64);
            textureSize = RTEditorGUI.Vector2Field("Texture Size", textureSize);

            if (GUI.changed)
            { // Texture has been changed

                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }
        }

        public override bool Calculate()
        {
            HeightMap = TextureGeneration.GenerateHeigthMapWave(xPeriod, yPeriod, turbPower, turbSize, textureSize);
            heightMapType.SetValue(HeightMap);

            return true;
        }
    }
}