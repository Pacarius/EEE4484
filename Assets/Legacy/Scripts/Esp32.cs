using System;
using UnityEngine;

public class Esp32 : MonoBehaviour{
    public GameObject[] Positive_Pins;
    public GameObject[] Negative_Pins;
    public PinInfo[] pinInfos;
}
[Serializable]
public struct PinInfo{
    public GameObject Pin;
    public string ID;
}