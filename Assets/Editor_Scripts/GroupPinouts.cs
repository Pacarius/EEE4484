using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GroupPinouts : MonoBehaviour
{
    public PinoutHolder[] pinoutHolders;  // For individual pinouts
    public GridPinoutHolder[] gridPinoutHolders;  // For grid pinouts

    public void Awake()
    {
        int count = 0;
        Breadboard board = gameObject.GetComponent<Breadboard>() ?? gameObject.AddComponent<Breadboard>();
        List<List<GameObject>> Rails = new();

        // Initialize Rails for PinoutHolders
        for (int i = 0; i < pinoutHolders.Length; i++) {
            Rails.Add(new List<GameObject>());
        }

        // Process regular PinoutHolders
        foreach (Transform pinout in transform)
        {
            for (int i = 0; i < pinoutHolders.Length; i++)
            {
                PinoutHolder holder = pinoutHolders[i];
                if (IsBetween(holder.start.transform.position, holder.end.transform.position, pinout.position))
                {
                    Rails[i].Add(pinout.gameObject);
                    count++;
                }
            }
        }

        // Process GridPinoutHolders, splitting along the z axis
        for (int i = 0; i < gridPinoutHolders.Length; i++)
        {
            Vector3 start = gridPinoutHolders[i].start.transform.position;
            Vector3 end = gridPinoutHolders[i].end.transform.position;

            // Collect GameObjects within the grid's column range
            List<List<GameObject>> columns = new();
            foreach (Transform pinout in transform)
            {
                if (IsBetweenGrid(start, end, pinout.position))
                {
                    bool added = false;
                    foreach (var column in columns)
                    {
                        if (IsInSameColumn(column[0].transform.position, pinout.position))
                        {
                            column.Add(pinout.gameObject);
                            added = true;
                            break;
                        }
                    }
                    if (!added)
                    {
                        columns.Add(new List<GameObject> { pinout.gameObject });
                    }
                }
            }

            // Add the collected GameObjects to Rails
            Rails.AddRange(columns);
        }

        // Add Hole component to each GameObject and set up the Breadboard reference and rail index
        for (int i = 0; i < Rails.Count; i++)
        {
            foreach (var pinout in Rails[i])
            {
                PinHole hole = pinout.GetComponent<PinHole>() ?? pinout.AddComponent<PinHole>();
                hole.breadboard = board;
                hole.railIndex = i;
                count++;
            }
        }

        Debug.Log(count);
        board.Rails = Rails.Select(a => new Rails(a.ToArray())).ToArray();
    }

    private bool IsBetween(Vector3 start, Vector3 end, Vector3 point)
    {
        // Check if the point is between start and end, now using the x axis
        return point.x >= Mathf.Min(start.x, end.x) && point.x <= Mathf.Max(start.x, end.x) &&
               point.z >= Mathf.Min(start.z, end.z) && point.z <= Mathf.Max(start.z, end.z);
    }

    private bool IsBetweenGrid(Vector3 start, Vector3 end, Vector3 point)
    {
        // Check if the point is within the grid bounds defined by start and end positions
        return point.x >= Mathf.Min(start.x, end.x) && point.x <= Mathf.Max(start.x, end.x) &&
               point.z >= Mathf.Min(start.z, end.z) && point.z <= Mathf.Max(start.z, end.z);
    }

    private bool IsInSameColumn(Vector3 position1, Vector3 position2)
    {
        // Check if two positions are in the same column based on a small tolerance
        return Mathf.Abs(position1.x - position2.x) < 0.0001f;
    }
}

[Serializable]
public struct PinoutHolder
{
    public GameObject start;
    public GameObject end;
}

[Serializable]
public struct GridPinoutHolder
{
    public GameObject start;
    public GameObject end;
}

// [CustomEditor(typeof(GroupPinouts))]
// public class GroupPinoutsEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
//         GroupPinouts script = (GroupPinouts)target;
//         if (GUILayout.Button("Spawn Children.")) script.Run();
//     }
// }