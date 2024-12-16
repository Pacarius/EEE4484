using UnityEngine;

public class MicroUsb : StateObjects
{
    internal override Vector3 Location => new (-0.0329900011f,-0.00100699998f,-0.00730000017f);
    public GameObject Real;
    public GameObject Fake;
     public override void OnTriggerEnter(Collider other)
    {
        if (other.name == "MicroUSB")
        {
            base.OnTriggerEnter(other);
            Destroy(Real);
            Fake.SetActive(true);
            HandGrab.SetActive(false);
            Destroy(this.gameObject);
        }
    }
}