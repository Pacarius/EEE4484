using Unity.VisualScripting;
using UnityEngine;

public class Esp32 : StateObjects
{
    public Vector3 Location = new Vector3(-0.213539228f, -0.0265290365f, -0.116301447f);
    internal override Vector3 _location() => Location;

    public override void OnTriggerEnter(Collider other)
    {
        if(other.name == "Esp32")
        {
           manager().SetEsp32(_location());
           Destroy(this.gameObject);
        }
    }
}
