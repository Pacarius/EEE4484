using UnityEngine;

public class SR04 : StateObjects
{
    public Vector3 Location = new(-0.471539199f, -0.0789000019f, 0.0166985393f);
    internal override Vector3 _location() => Location;
    public override void OnTriggerEnter(Collider other)
    {
        if (other.name == "SR04")
        {
            manager().setSR04(_location());
            Destroy(this.gameObject);
        }
    }
}