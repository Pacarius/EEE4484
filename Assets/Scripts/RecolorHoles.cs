using System.Collections.Generic;
using System;
using UnityEngine;

public class RecolorHoles : MonoBehaviour
{
    public Rails[] Rails;
    public Material passthroughMaterial;
    public Material positiveMaterial;
    public Material negativeMaterial;
    public Material avoidMaterial;
    private void Start()
    {
 //       Color();
    }
    public void Color()
    {
        foreach(var r in Rails)
        {
            foreach (var g in r.Pinouts)
            {
                g.GetComponent<Renderer>().material = railMatch(r.RailType);
            }
        }
    }
    public Material railMatch(PinType type)
    {
        switch (type)
        {
            case PinType.Passthrough: return passthroughMaterial;
            case PinType.Positive: return positiveMaterial;
            case PinType.Negative: return negativeMaterial;
            case PinType.Avoid: return avoidMaterial;
            default: return null;
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
}
public enum PinType
{
    Passthrough,
    Positive,
    Negative,
    Avoid,
    Default
}