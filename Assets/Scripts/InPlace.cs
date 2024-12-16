using Oculus.Interaction;
using System;
using UnityEngine;

public class InPlace : MonoBehaviour
{
    public SnapInteractable interactable = null;
    private void Awake()
    {
        interactable.WhenStateChanged += debug;
    }
    void debug(InteractableStateChangeArgs args)
    {
        Debug.Log($"{args.NewState}");
    }
}
