using System.Collections.Generic;
using System;
using UnityEngine;

public class RecolorHoles : MonoBehaviour
{

    public Rails[] Rails;
    public void Color()
    {
        foreach (var r in Rails)
        {
            foreach (var g in r.Pinouts)
            {
                g.GetComponent<Renderer>().material = r.railMatch;
            }
        }
    }
}
[Serializable]
public class Rails
{
    public GameObject[] Pinouts;
    public PinType RailType;
    public Rails(GameObject[] pins)
    {
        Pinouts = pins;
    }
    public Material railMatch
    {
        get
        {
            return PinHelper.pinMaterial(RailType);
        }
    }
}
public enum PinType
{
    Passthrough,
    Positive,
    Negative,
    Avoid,
    Default
}
public static class PinHelper{
    public static Material pinMaterial(PinType type){
        return type switch
        {
            PinType.Passthrough => StateManager.Singleton.passthroughMaterial,
            PinType.Positive => StateManager.Singleton.positiveMaterial,
            PinType.Negative => StateManager.Singleton.negativeMaterial,
            PinType.Avoid => StateManager.Singleton.avoidMaterial,
            _ => null,
        };
    }
}