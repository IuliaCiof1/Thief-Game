using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public struct Weights
{
    [Range(0,10)] public int intersectionWeight;
    [Range(0,10)] public int roadStraightWeight;
    [Range(0,10)] public int waterWeight;
    [Range(0,10)] public int waterBendWeight;
    [Range(0, 10)] public int building;
    [Range(0, 10)] public int lawn;
    public int GetWeight(Attribute_ a)
    {
        if(a==Attribute_.Intersection)
            return intersectionWeight;
        else if(a==Attribute_.RoadStraight)
            return roadStraightWeight;
        else if(a==Attribute_.Water)
            return waterWeight;
        else if(a==Attribute_.WaterBend)
            return waterBendWeight;
        else if (a == Attribute_.Lawn)
            return lawn;
        else if (a == Attribute_.Building)
            return building;

        return 0;
    }
}
public enum Attribute_ {Intersection, RoadStraight, Water, WaterBend, Lawn, Building};
