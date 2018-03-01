using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoadNetwork
{
    public class Segment
    {
        Vector3 startPoint;
        Vector3 endPoint;

        List<Vector3> controlPoints;
    }


    public class SimpleRoadGeneration
    {
        public static void GenerateRoad(Vector3 start, Vector3 end)
        {

        }

        List<Segment> GlobalGoals(Segment s)
        {
            List<Segment> newSegments = new List<Segment>();



            return newSegments;
        }
    }
}