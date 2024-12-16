using UnityEngine;

public abstract class StateObjects : MonoBehaviour
{
    internal abstract Vector3 _location();
    public abstract void OnTriggerEnter(Collider other);
    internal StateManager manager() => StateManager.Singleton;
}
