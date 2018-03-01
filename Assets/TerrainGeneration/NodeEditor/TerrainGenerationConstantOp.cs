using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.TerrainGenerator
{
    [Node(false, "Terrain/Constant")]
    public class TerrainGenerationConstantOp : Node
    {
        public const string ID = "terrainConstantOp";
        public override string GetID { get { return ID; } }
        public override bool ContinueCalculation
        {
            get
            {
                return true;
            }
        }
        public override string Title { get { return "Terrain Constant Operation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 150); } }

        [ValueConnectionKnob("Terrain", Direction.In, "Terrain")]
        public ValueConnectionKnob inKnob;
        [ValueConnectionKnob("Terrain", Direction.Out, "Terrain")]
        public ValueConnectionKnob outKnob;

        TerrainFunc input;

        public override void NodeGUI()
        {
            
            inKnob.DisplayLayout();
            

            outKnob.DisplayLayout();
            

            if (GUI.changed)
            {
                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }

        }

        public override bool Calculate()
        {

            input = inKnob.GetValue<TerrainFunc>();

            outKnob.SetValue(new ConstantOp(input, 2));

            return true;
        }

        public class ConstantOp : TerrainFunc
        {
            TerrainFunc f;
            float val;

            public ConstantOp(TerrainFunc f, float val)
            {
                this.f = f;
                this.val = val;
                generateFunc = Div;
            }

            float Div(float x, float y)
            {
                return (f.generateFunc(x, y) / val);
            }
        }
    }
}