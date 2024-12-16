using System;
using UnityEngine;

public class PostStates : MonoBehaviour
{
    public GameObject SR04;
    public GameObject Wood;
    public Action<float> OnSR04Hit = delegate { };
    void Awake(){
        OnSR04Hit += OnHit;
    }
    void OnHit(float dist) =>  Debug.Log($"SR04 hit Wood at {dist}m"); 
    void FixedUpdate()
    {
        RunSR04();
    }
    void RunSR04(){
        if (SR04 != null && Wood != null)
        {
            Vector3 directionToWood = Wood.transform.position - SR04.transform.position;
            float distanceToWood = Vector3.Distance(SR04.transform.position, Wood.transform.position);

            // Check if Wood is within 10 degrees in front of SR04
            float angle = Vector3.Angle(SR04.transform.forward, directionToWood);
            if (angle <= 10.0f)
            {
                if (Physics.Raycast(SR04.transform.position, directionToWood, out RaycastHit hit, distanceToWood))
                {
                    if (hit.collider.gameObject == Wood)
                    {
                        OnSR04Hit(distanceToWood);
                    }
                }
            }
        }
    }
}