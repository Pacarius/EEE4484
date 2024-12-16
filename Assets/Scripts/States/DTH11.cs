using UnityEngine;

public class DHT11 : StateObjects
{

    internal override Vector3 Location => new Vector3(-0.0292000007f, -0.0119000003f, -0.0322000012f);
    public override void OnTriggerEnter(Collider other)
    {
        if (other.name == "DHT11")
        {
            base.OnTriggerEnter(other);
            manager.Activate(_sceneObject);
            Destroy(gameObject);
        }
    }
}