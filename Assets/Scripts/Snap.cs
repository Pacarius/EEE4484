using Oculus.Interaction;
using System;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public GameObject target;
    public Vector3 Location;
    public bool Oneshot = false;
    void FixedUpdate(){
            target.transform.localPosition = Location;
            target.transform.rotation = Quaternion.identity;
            if(Oneshot) Destroy(this);
    }
}
