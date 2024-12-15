using UnityEditor;
using UnityEngine;

public class InstantiateEsp32 : MonoBehaviour{
    public Transform Start;
    public Transform End;
    public int Count;
    public GameObject Source;
    public void Run(){
        Vector3 Distance = End.position - Start.position;
        PinList pinList = gameObject.GetComponent<PinList>() ?? gameObject.AddComponent<PinList>();
        for (int i = 0; i <= Count; i++){
            GameObject gameObject = Instantiate(Source, Start.position + Distance/Count * i, Quaternion.identity, parent:transform);
            gameObject.name = "Pin_" + pinList.pins.Count;
            Esp32Pin pin = gameObject.AddComponent<Esp32Pin>();
            pin.PinID = (short)pinList.pins.Count;
            pinList.pins.Add(pin);
        }
    }
}
[CustomEditor(typeof(InstantiateEsp32))]
public class _InstantiateArray : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        InstantiateEsp32 script = (InstantiateEsp32)target;
        if(GUILayout.Button("Spawn Children.")) script.Run();
    }
}