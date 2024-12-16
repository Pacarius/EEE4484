using System;
using UnityEngine;

public abstract class StateObjects : MonoBehaviour
{
    // internal abstract Vector3 _location();
    public virtual void OnTriggerEnter(Collider other){
        try{
            manager.States[state].targetsCompleted[uuid] = true;
        } catch (Exception e){
            Debug.Log(e);
        }
    }
    public GameObject Identity = null;
    public GameObject HandGrab = null;
    internal StateManager manager { get { return StateManager.Singleton; } }
    internal abstract Vector3 Location { get; }
    internal SceneObject _sceneObject;
    public int uuid = 0;
    public int state = -1;
    void Awake(){
        _sceneObject = new SceneObject(Identity, HandGrab, Location);
    }
}
