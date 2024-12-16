using UnityEngine;

public class DHT11 : StateObjects
{
    public Vector3 Location = new(-0.0290392339f, -0.063000001f, -0.0325014591f);
    internal override Vector3 _location() => Location;
    public override void OnTriggerEnter(Collider other)
    {
        if (other.name == "DHT11")
        {
            manager().SetDHT11(_location());
            Destroy(this.gameObject);
        }
    }
}