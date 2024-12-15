using System;
using System.Collections.Generic;
using UnityEngine;

public class Breadboard : MonoBehaviour
{
    public Rails[] Rails;
    public Material passthroughMaterial;
    public Material positiveMaterial;
    public Material negativeMaterial;
    public Material avoidMaterial;
    public Material defaultMaterial;
}
public class Rails
{
    public GameObject[] Pinouts;
    public PinType RailType;
    public event Action<GameObject> OnConnected;
    public event Action<GameObject> OnDisconnected;

    public Rails(GameObject[] pins)
    {
        Pinouts = pins;
        OnConnected = DefaultOnConnected;
        OnDisconnected = DefaultOnDisconnected;
    }

    public void Connect(GameObject pin)
    {
        // Add the pin to the Pinouts array (you might need to resize the array)
        // For simplicity, let's assume Pinouts is a List<GameObject>
        List<GameObject> pinList = new(Pinouts)
        {
            pin
        };
        Pinouts = pinList.ToArray();

        // Trigger the OnConnected event
        OnConnected?.Invoke(pin);
        BroadcastPinInformation();
    }

    public void Disconnect(GameObject pin)
    {
        // Remove the pin from the Pinouts array
        List<GameObject> pinList = new(Pinouts);
        if (pinList.Remove(pin))
        {
            Pinouts = pinList.ToArray();

            // Trigger the OnDisconnected event
            OnDisconnected?.Invoke(pin);
            BroadcastPinInformation();
        }
    }
    private void BroadcastPinInformation()
    {
        foreach (var pin in Pinouts)
        {
            // Get the Pin component to access its information
            var pinInfo = pin.GetComponent<Esp32Pin>();
            if (pinInfo != null)
            {
                Debug.Log($"Pin {pinInfo.PinName} (ID: {pinInfo.PinID}, Type: {pinInfo.pinType}) is on the rail.");
            }
        }
    }
    private static void DefaultOnConnected(GameObject pin)
    {
        Debug.Log($"Pin {pin.name} has been connected.");
    }

    private static void DefaultOnDisconnected(GameObject pin)
    {
        Debug.Log($"Pin {pin.name} has been disconnected.");
    }
}