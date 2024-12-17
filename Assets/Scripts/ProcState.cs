using UnityEngine;

public class ProcState : MonoBehaviour{
    public void Activate(){
        StateManager.Singleton.Activate();
        gameObject.SetActive(false);
    }
}