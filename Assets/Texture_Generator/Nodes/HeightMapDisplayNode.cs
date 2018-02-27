using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/MapDisplay")]
    public class HeightMapDisplayNode : Node
    {
        public const string ID = "mapDisplay";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "Map Display"; } }
        public override Vector2 DefaultSize { get { return new Vector2(150, 50); } }
        public override bool AutoLayout { get { return true; } }

        [ValueConnectionKnob("HeightMap", Direction.In, "HeightMap")]
        public ValueConnectionKnob inputKnob;

        HeightMap heightMap;
        
        MapDisplay mapDisplay;

        private void Awake()
        {
            mapDisplay = FindObjectOfType<MapDisplay>();
        }



        public override void NodeGUI()
        {
            inputKnob.DisplayLayout();
            

        }

        public override bool Calculate()
        {

            heightMap = inputKnob.connected() ? inputKnob.GetValue<HeightMap>() : null;
            mapDisplay.DrawMesh(MeshGeneration.GenerateMeshFromHeigthMap(heightMap.heightMap, 20));

            return true;
        }
    }
}