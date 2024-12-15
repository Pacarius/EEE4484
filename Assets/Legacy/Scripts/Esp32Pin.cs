using UnityEngine;

public class Esp32Pin : MonoBehaviour
{
    public short PinID = -1;
    public string PinName = null;
    public PinType pinType = PinType.Passthrough;
}

public enum PinType
{
    Passthrough,
    Positive,
    Negative,
    Avoid,
    Default
}