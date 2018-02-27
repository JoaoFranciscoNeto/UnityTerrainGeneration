using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "HeightMap/Operation")]
    public class HeightMapOperation : Node
    {
		public const string ID = "heightMapOperation";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "HeightMap Operation"; } }
        public override Vector2 DefaultSize { get { return new Vector2(250, 100); } }

        public enum OpType { Add, Multiply }
        public OpType type = OpType.Multiply;

        [ValueConnectionKnob("Input 1", Direction.In, "HeightMap")]
        public ValueConnectionKnob heightMapInput1;
        [ValueConnectionKnob("Input 2", Direction.In, "HeightMap")]
        public ValueConnectionKnob heightMapInput2;
        
        [ValueConnectionKnob("Output", Direction.Out, "HeightMap")]
        public ValueConnectionKnob outputNode;

        public HeightMap input1;
        public HeightMap input2;

        public HeightMap output;

        public override void NodeGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            heightMapInput1.DisplayLayout();
            heightMapInput2.DisplayLayout();

            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            outputNode.DisplayLayout();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            type = (OpType)RTEditorGUI.EnumPopup(new GUIContent("Operation Type", "The type of calculation performed on Input 1 and Input 2"), type);

            if (GUI.changed)
                NodeEditor.curNodeCanvas.OnNodeChange(this);
        }

        public override bool Calculate()
        {
            input1 = heightMapInput1.GetValue<HeightMap>();
            input2 = heightMapInput2.GetValue<HeightMap>();

            switch (type)
            {
                case OpType.Add:
                    Add();
                    break;
                case OpType.Multiply:
                    Multiply();
                    break;
            }

            outputNode.SetValue(output);
            return true;
        }

        void Add()
        {
            float[,] newHeightMap = new float[input1.heightMap.GetLength(0), input1.heightMap.GetLength(1)];

            for (int x = 0; x < newHeightMap.GetLength(0); x++)
            {
                for (int y = 0; y < newHeightMap.GetLength(1); y++)
                {
                    newHeightMap[x, y] = input1.heightMap[x, y] + input2.heightMap[x, y];
                }
            }

            output = new HeightMap(newHeightMap);
        }

        void Multiply()
        {
            float[,] newHeightMap = new float[input1.heightMap.GetLength(0), input1.heightMap.GetLength(1)];

            for (int x = 0; x < newHeightMap.GetLength(0); x++)
            {
                for (int y = 0; y < newHeightMap.GetLength(1); y++)
                {
                    newHeightMap[x, y] = input1.heightMap[x, y] * input2.heightMap[x, y];
                }
            }

            output = new HeightMap(newHeightMap);

        }
    }
}