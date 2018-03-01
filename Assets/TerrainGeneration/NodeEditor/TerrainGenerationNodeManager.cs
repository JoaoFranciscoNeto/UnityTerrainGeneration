using System;
using UnityEngine;

namespace NodeEditorFramework.TerrainGenerator
{

    public class TerrainType : ValueConnectionType
    {
        public override string Identifier { get { return "Terrain"; } }
        public override Type Type { get { return typeof(TerrainFunc); } }
        public override Color Color { get { return Color.blue; } }
    }

    public class TerrainFunc
    {
        public Func<float, float, float> generateFunc;
    }
}
