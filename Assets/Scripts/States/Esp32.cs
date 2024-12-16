using Unity.VisualScripting;
using UnityEngine;

public class Esp32 : StateObjects
{
    internal override Vector3 Location => new(-0.213539228f, -0.0265290365f, -0.116301447f);
    public override void OnTriggerEnter(Collider other)
    {
        if (other.name == "Esp32")
        {
            base.OnTriggerEnter(other);
            manager.Activate(_sceneObject);
            manager._holes.Color();
            Destroy(gameObject);
        }
    }
}
