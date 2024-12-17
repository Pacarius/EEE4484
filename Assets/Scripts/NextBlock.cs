using UnityEngine;

public class NextBlock : MonoBehaviour
{
    public GameObject Identity;
    public GameObject Next;
    public int targetState = -2;
    private void Start()
    {
        StateManager.Singleton.stateObjects[targetState].Add(Identity);
    }
}
