using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/HeightMap Info")]
    public class HeightMapInfoNode : Node
    {
        public const string ID = "heightMapInfoNode";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "HeightMap Info"; } }
        public override Vector2 MinSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        [ValueConnectionKnob("HeightMap", Direction.In, "HeightMap")]
        public ValueConnectionKnob inputKnob;

        [System.NonSerialized]
        public HeightMap heightMap;


        public override void NodeGUI()
        {
            inputKnob.DisplayLayout(new GUIContent("HeightMap" + (heightMap != null ? ":" : " (null)"), "The HeightMap to display information about."));
            if (heightMap != null)
            {
                RTTextureViz.DrawTexture(heightMap.texture, 64);
                GUILayout.Label("Size: " + heightMap.texture.width + "x" + heightMap.texture.height + "");
                GUILayout.Label("Format: " + heightMap.texture.format);
                GUILayout.Label("Height Multiplier: " + heightMap.heightMultiplier);
            }
        }

        public override bool Calculate()
        {
            heightMap = inputKnob.connected() ? inputKnob.GetValue<HeightMap>() : null;
            return true;
        }
    }
}