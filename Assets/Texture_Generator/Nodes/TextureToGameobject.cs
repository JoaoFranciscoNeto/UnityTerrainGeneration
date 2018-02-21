
using UnityEngine;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.TextureGenerator
{
    [Node(false, "Texture/OutputGameObject")]
    public class TextureToGameobject : Node
    {
        public const string ID = "texToGameobject";
        public override string GetID { get { return ID; } }

        public override string Title { get { return "TextureToGameobject"; } }
        public override Vector2 DefaultSize { get { return new Vector2(100, 100); } }

        [ValueConnectionKnob("HeightMap", Direction.In, "HeightMap")]
        public ValueConnectionKnob inputKnob;

        [System.NonSerialized]
        public GameObject gameObject;

        [System.NonSerialized]
        public float[,] heightMap;

        MapDisplay mapDisplay;
        




        public override void NodeGUI()
        {
            inputKnob.DisplayLayout();

            gameObject = RTEditorGUI.ObjectField(gameObject, true);

            if (GUI.changed)
            { // Texture has been changed
                mapDisplay = gameObject.GetComponent<MapDisplay>();
                if (mapDisplay == null) { 
                    Debug.LogError("GameObject needs a MapDisplay component");
                }

                NodeEditor.curNodeCanvas.OnNodeChange(this);
            }
        }

        public override bool Calculate()
        {
            heightMap = inputKnob.connected() ? inputKnob.GetValue < float[,]>() : null;
            if (gameObject)
            {
                mapDisplay = gameObject.GetComponent<MapDisplay>();
                if (mapDisplay)
                {
                    mapDisplay.DrawMesh(MeshGeneration.GenerateMeshFromHeigthMap(heightMap, 10));
                }
            }
            return true;
        }
    }
}
