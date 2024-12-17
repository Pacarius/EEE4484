using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
[Serializable]
public struct CanvasState{
    public int stateIndex;
    public GameObject[] canvases;
}
public class CanvasManager : MonoBehaviour
{
    public CanvasState[] stateCanvases;
    bool Initialised = false;
    int currentCanvas = 0;
    Dictionary<int, List<GameObject>> stateCanvasesDict = new();
    int state;
    void Start(){
        foreach(CanvasState StateCanvases in stateCanvases){
            stateCanvasesDict.Add(StateCanvases.stateIndex, new List<GameObject>(StateCanvases.canvases));
            foreach(GameObject canvas in StateCanvases.canvases){
                canvas.SetActive(false);
            }
        }
        StateManager.Singleton.OnStateChange += HandleStateChange;
        Initialised = true;
    }

    private void OnEnable()
    {
        if(Initialised)
        StateManager.Singleton.OnStateChange += HandleStateChange;
    }

    private void OnDisable()
    {
        StateManager.Singleton.OnStateChange -= HandleStateChange;
    }

    private void HandleStateChange(int newState)
    {
        state = newState;
        SwapStates();
    }

    public void SwapStates()
    {
        foreach (var kvp in stateCanvasesDict)
        {
             for (int i = 0; i < kvp.Value.Count; i++)
            {
                kvp.Value[i].SetActive(kvp.Key == state && i == 0);
            }
        }
        currentCanvas = 0;
    }
    public void SwapCanvases(){
        currentCanvas++;
        if(currentCanvas == stateCanvasesDict.Count){
            currentCanvas = 0;
        }
        foreach (var kvp in stateCanvasesDict)
        {
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                GameObject canvas = kvp.Value[i];
                canvas.SetActive(kvp.Key == state && i == currentCanvas);
           }
        }
    }
}
[CustomEditor(typeof(CanvasManager))]
public class CanvasManagerEditor : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        CanvasManager canvasManager = (CanvasManager)target;
        if(GUILayout.Button("Swap Canvases")){
            canvasManager.SwapCanvases();
        }
    }
}