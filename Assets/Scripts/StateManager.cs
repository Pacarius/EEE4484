using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public State[] _States;
    private static StateManager _singleton;
    public RecolorHoles _holes;
    public Material passthroughMaterial;
    public Material positiveMaterial;
    public Material negativeMaterial;
    public Material avoidMaterial;
    public Dictionary<int, State> States = new(); 
    public int state { get; private set; } = 0;
    System.Action Initialise = () => { };
    public TextMeshProUGUI StateText;
    public TextMeshProUGUI DistanceText;
    List<string> stateTexts = new(){
        "Hi!, welcome to today's lab. We're going to be testing out some sensors today with the help of an Esp32 Module and a breadboard. First, let's connect the Esp32 to the breadboard. The Esp32 is a microcontroller that can be used to connect to the internet and interact with other devices.",
        "Next up, you're gonna see a couple of sensors pop up. Connect those to the breadboard."
    };
    public static StateManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(StateManager)} instance already exists, destroying object!");
                Destroy(value);
            }
        }
    }
    void Awake()
    {
        Singleton = this;
        foreach (State state in _States)
        {
            try{
                States.Add(state.ID, state);
                Initialise += state.Initialise;
            } catch (ArgumentException){
                Debug.Log($"Possible duplicate of {state.Name} at {state.ID}.");
            }
        }
        Initialise();
        // DeactivateFutureStates();
    }
    // void DeactivateFutureStates(){
    //     foreach(State _state in States.Values){
    //         if(_state.ID > state)
    //         foreach(GameObject target in _state.targets){
    //             target.SetActive(false);
    //         }
    //     }
    // }
    void ActivateState(){
        if(States.ContainsKey(state))
        foreach(GameObject target in States[state].targets){
            target.SetActive(true);
            StateObjects so = target.GetComponent<StateObjects>();
            if(so) so.Identity.SetActive(true);
        }
        StateText.text = stateTexts[state];
    }
    public void Activate(SceneObject target)
    {
        Snap snap = target.Identity.AddComponent<Snap>();
        snap.Location = target.Location;
        snap.target = target.Identity;
        snap.Oneshot = false;
        target.HandGrab.SetActive(false);
        if(States[state].targetsCompleted.TrueForAll(x => x)){
            state++;
            ActivateState();
        }
    }
    public void Activate(){
        state++;
        ActivateState();
    }
}
[CustomEditor(typeof(StateManager))]
public class StateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StateManager stateManager = (StateManager)target;

        if (GUILayout.Button("Activate State"))
        {
            stateManager.Activate();
        }
    }
}
public class SceneObject
{
    public GameObject Identity = null;
    public GameObject HandGrab = null;
    public Vector3 Location = Vector3.zero;
    public SceneObject(GameObject identity, GameObject handGrab, Vector3 location)
    {
        Identity = identity;
        HandGrab = handGrab;
        Location = location;
    }
}
[Serializable]
public class State{
    public string Name = "";
    public int ID = -1;
    public GameObject[] targets;
    [HideInInspector]
    public List<bool> targetsCompleted;
    public void Initialise(){
        Debug.Log($"Initialising {Name} at {ID}.");
        targetsCompleted = Enumerable.Repeat(false, targets.Length).ToList();
        int uuid = 0;
        // targetsCompleted = new(targets.Length); 
        foreach(GameObject target in targets){
            StateObjects stateObject = target.GetComponent<StateObjects>();
            if(stateObject != null){
                stateObject.uuid = uuid;
                stateObject.state = ID;
                if(ID > StateManager.Singleton.state) stateObject.Identity.SetActive(false);
            }
            if(ID > StateManager.Singleton.state)                
                target.SetActive(false);
            uuid++;
        }
       
    }
}