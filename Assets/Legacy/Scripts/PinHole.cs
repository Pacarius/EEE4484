using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHole : MonoBehaviour
{
    public Breadboard breadboard;
    public int railIndex;

    private Material passthroughMaterial;
    private Material positiveMaterial;
    private Material negativeMaterial;
    private Material avoidMaterial;
    private Material defaultMaterial;

    private List<Renderer> cachedRenderers = new();

    private void Start()
    {
        // Subscribe to the events
        breadboard.Rails[railIndex].OnConnected += HandlePinConnected;
        breadboard.Rails[railIndex].OnDisconnected += HandlePinDisconnected;
        passthroughMaterial = breadboard.passthroughMaterial;
        positiveMaterial = breadboard.positiveMaterial;
        negativeMaterial = breadboard.negativeMaterial;
        avoidMaterial = breadboard.avoidMaterial;
        defaultMaterial = breadboard.defaultMaterial;

        // Cache Renderer components
        foreach (var pinout in breadboard.Rails[railIndex].Pinouts)
        {
            Renderer renderer = pinout.GetComponent<Renderer>();
            if (renderer != null)
            {
                cachedRenderers.Add(renderer);
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events
        breadboard.Rails[railIndex].OnConnected -= HandlePinConnected;
        breadboard.Rails[railIndex].OnDisconnected -= HandlePinDisconnected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pin"))
        {
            Debug.Log("PinHole: OnTriggerEnter");
            GameObject pin = other.gameObject;
            breadboard.Rails[railIndex].Connect(pin);
        }
        if(other.CompareTag("WirePin"))
        {
            Debug.Log("PinHole: OnTriggerEnter");
            GameObject wirePin = other.gameObject;
            wirePin.GetComponent<WirePin>().SetRailInfo(railIndex, breadboard.Rails[railIndex].RailType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pin"))
        {
            GameObject pin = other.gameObject;
            breadboard.Rails[railIndex].Disconnect(pin);
        }
        if(other.CompareTag("WirePin"))
        {
            GameObject wirePin = other.gameObject;
            wirePin.GetComponent<WirePin>().SetRailInfo(-1, PinType.Default);
        }
    }

    private void HandlePinConnected(GameObject pin)
    {
        Esp32Pin pinComponent = pin.GetComponent<Esp32Pin>();
        if (pinComponent != null)
        {
            Debug.Log($"Pin {pinComponent.PinName} connected to rail {railIndex}");
            StartCoroutine(ChangePinHoleMaterials(pinComponent.pinType));
            breadboard.Rails[railIndex].RailType = pinComponent.pinType;
        }
    }

    private void HandlePinDisconnected(GameObject pin)
    {
        Esp32Pin pinComponent = pin.GetComponent<Esp32Pin>();
        if (pinComponent != null)
        {
            Debug.Log($"Pin {pinComponent.PinName} disconnected from rail {railIndex}");
            StartCoroutine(ChangePinHoleMaterials(PinType.Default));
            breadboard.Rails[railIndex].RailType = PinType.Default;
        }
    }

    private IEnumerator ChangePinHoleMaterials(PinType pinType)
    {
        Material newMaterial = passthroughMaterial;

        switch (pinType)
        {
            case PinType.Passthrough:
                newMaterial = passthroughMaterial;
                break;
            case PinType.Positive:
                newMaterial = positiveMaterial;
                break;
            case PinType.Negative:
                newMaterial = negativeMaterial;
                break;
            case PinType.Avoid:
                newMaterial = avoidMaterial;
                break;
            case PinType.Default:
                newMaterial = defaultMaterial;
                break;
        }

        // Change materials for cached renderers
        foreach (var renderer in cachedRenderers)
        {
            if (renderer != null)
            {
                renderer.material = newMaterial;
                yield return null; // Spread the work over multiple frames
            }
        }
    }
}