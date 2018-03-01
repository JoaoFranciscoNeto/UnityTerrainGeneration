using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TerrainGenerator
{
    [Node(false, "Terrain/Add")]
    public class TerrainGenerationAdd : Node
    {
        public const string ID = "terrainAdd";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }
        public override string Title { get { return "Terrain Add"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }

        [ValueConnectionKnob("Terrain", Direction.In, "Terrain")]
        public ValueConnectionKnob inKnob1;
        [ValueConnectionKnob("Terrain", Direction.In, "Terrain")]
        public ValueConnectionKnob inKnob2;
        [ValueConnectionKnob("Terrain", Direction.Out, "Terrain")]
        public ValueConnectionKnob outKnob;

        TerrainFunc input1;
        TerrainFunc input2;

        public override void NodeGUI()
        {

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            inKnob1.DisplayLayout();
            inKnob2.DisplayLayout();

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            outKnob.DisplayLayout();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            
            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {

            input1 = inKnob1.GetValue<TerrainFunc>();
            input2 = inKnob2.GetValue<TerrainFunc>();

            outKnob.SetValue(new AddTerrain(input1, input2));

            return true;
        }

        public class AddTerrain : TerrainFunc
        {
            TerrainFunc f1;
            TerrainFunc f2;

            public AddTerrain(TerrainFunc f1, TerrainFunc f2)
            {
                this.f1 = f1;
                this.f2 = f2;

                generateFunc = Add;
            }

            float Add(float x, float y)
            {
                float v1 = f1.generateFunc(x, y);
                float v2 = f2.generateFunc(x, y);
                return v1+v2;
            }
        }
    }
}