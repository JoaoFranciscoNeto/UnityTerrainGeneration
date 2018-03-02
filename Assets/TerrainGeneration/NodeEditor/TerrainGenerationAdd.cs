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

        float influence = .5f;

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

            influence = RTEditorGUI.Slider("Influence", influence, 0, 1);


            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {

            input1 = inKnob1.GetValue<TerrainFunc>();
            input2 = inKnob2.GetValue<TerrainFunc>();

            outKnob.SetValue(new AddTerrain(input1, input2, influence));

            return true;
        }

        public class AddTerrain : TerrainFunc
        {
            TerrainFunc f1;
            TerrainFunc f2;
            float influence;

            public AddTerrain(TerrainFunc f1, TerrainFunc f2, float influence)
            {
                this.f1 = f1;
                this.f2 = f2;
                this.influence = influence;

                generateFunc = Add;
            }

            float Add(float x, float y)
            {
                float v1 = f1.generateFunc(x, y);
                float v2 = f2.generateFunc(x, y);
                return (v1*influence) + (v2*(1-influence));
            }
        }
    }
}