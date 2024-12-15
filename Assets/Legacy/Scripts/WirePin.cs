using UnityEngine;

public class WirePin : MonoBehaviour
{
    public int RailIndex { get; private set; }
    public PinType RailType { get; private set; }

    public void SetRailInfo(int railIndex, PinType railType)
    {
        RailIndex = railIndex;
        RailType = railType;
        Debug.Log($"WirePin: Rail index set to {railIndex}, Rail type set to {railType}");
    }
}