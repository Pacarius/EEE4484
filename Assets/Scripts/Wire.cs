using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public struct WirePair
{
    public GameObject startCube;
    public GameObject endCube;
    public PinType wireType;

    public WirePair(GameObject start, GameObject end)
    {
        startCube = start;
        endCube = end;
        wireType = PinType.Default;
    }
}

public class Wire : MonoBehaviour
{
    public Material passthroughMaterial;
    public Material positiveMaterial;
    public Material negativeMaterial;
    public Material avoidMaterial;
    public GameObject wirePrefab; // Prefab for the wire (e.g., a stretched cylinder)
    public WirePair[] wirePairs; // List of wire pairs

    public float heightOffset = 0.0f; // Initial height offset

    public void GenerateWires()
    {
        foreach (WirePair pair in wirePairs)
        {
            GenerateWire(pair.startCube, pair.endCube, mterial(pair.wireType));
        }
    }

    public Material mterial(PinType type)
    {
        return type switch
        {
            PinType.Passthrough => passthroughMaterial,
            PinType.Positive => positiveMaterial,
            PinType.Negative => negativeMaterial,
            PinType.Avoid => avoidMaterial,
            _ => null,
        };
    }

    private void GenerateWire(GameObject startCube, GameObject endCube, Material material)
    {
        if (startCube == null || endCube == null)
        {
            Debug.LogError("Start or End cube is null.");
            return;
        }

        Vector3 start = startCube.transform.position; // Start position
        Vector3 end = endCube.transform.position; // End position

        // Step 1: Move straight up to avoid obstacles
        Vector3 midPoint1 = start + new Vector3(0, 0.02f + heightOffset, 0); // Go up by 0.05 units + height offset

        // Step 2: Move horizontally or vertically to align with the end cube
        Vector3 midPoint2 = new Vector3(end.x, midPoint1.y, start.z); // Horizontal movement first
        Vector3 midPoint3 = new Vector3(end.x, midPoint1.y, end.z);   // Vertical movement to reach end

        // Step 3: Create the path
        List<Vector3> pathPoints = new List<Vector3> { start, midPoint1, midPoint2, midPoint3, end };

        // Calculate the midpoint between start and end points for the parent GameObject
        Vector3 parentPosition = (start + end) / 2;
        GameObject wireParent = new GameObject("WireParent");
        wireParent.transform.position = parentPosition;

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            CreateStraightWire(pathPoints[i], pathPoints[i + 1], material, wireParent);
        }

        heightOffset += 0.02f; // Increment the height offset for the next wire
    }

    private void CreateStraightWire(Vector3 start, Vector3 end, Material wireMaterial, GameObject parent)
    {
        Vector3 direction = (end - start).normalized;
        float distance = Vector3.Distance(start, end);

        GameObject wire = Instantiate(wirePrefab, start + direction * (distance / 2), Quaternion.identity);
        wire.transform.LookAt(end);
        wire.transform.localScale = new Vector3(0.02f, 0.02f, distance); // Adjust wire thickness and length

        // Set the wire color to the stored random color
        Renderer wireRenderer = wire.transform.GetChild(0).GetComponent<Renderer>();
        if (wireRenderer != null)
        {
            wireRenderer.material = wireMaterial;
        }

        // Parent the wire to the empty GameObject
        wire.transform.parent = parent.transform;
    }
}

[CustomEditor(typeof(Wire))]
public class WireEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Wire wireGenerator = (Wire)target;

        if (GUILayout.Button("Generate Wire"))
        {
            wireGenerator.GenerateWires();
        }
    }
}