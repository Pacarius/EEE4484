using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public int state { get; private set; } = -1;
    System.Action Initialise = () => { Singleton.OnStateChange(Singleton.state); Debug.Log($"Broadcasted state {Singleton.state}.");};
    public TextMeshProUGUI StateText;
    public TextMeshProUGUI DistanceText;
    public Action<int> OnStateChange = (int state) => { };
    public Dictionary<int, List<GameObject>> stateObjects = new Dictionary<int, List<GameObject>>();
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
            stateObjects.Add(state.ID, new());
        }
        StartCoroutine(LateInit());
        // DeactivateFutureStates();
    }
    IEnumerator LateInit(){
        yield return new WaitForSeconds(1);
        Initialise();
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
        foreach(KeyValuePair<int, List<GameObject>> pair in stateObjects)
        {
            bool StateMatch = pair.Key == state;
            {
                foreach(GameObject target in pair.Value)
                {
                    target.SetActive(StateMatch);
                }
            }
        }
        Debug.Log($"State {state} activated.");
        OnStateChange(state);
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