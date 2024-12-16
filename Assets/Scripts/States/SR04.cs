using UnityEngine;

public class SR04 : StateObjects
{
    internal override Vector3 Location => new Vector3(-0.49000001f, 0.0121999998f, 0.0166985393f);
    public override void OnTriggerEnter(Collider other)
    {

        if (other.name == "SR04")
        {
            base.OnTriggerEnter(other);
            manager.Activate(_sceneObject);
            Destroy(gameObject);
        }
    }
}