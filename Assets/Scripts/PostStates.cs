using System;
using UnityEngine;

public class PostStates : MonoBehaviour
{
    public SensorText sensorText;
    public GameObject SR04;
    public GameObject Wood;
    public bool AirConditionerOn = false;
    void FixedUpdate()
    {
        RunSR04();
    }
    void RunSR04(){
        if (SR04 != null && Wood != null)
        {
            Vector3 directionToWood = Wood.transform.position - SR04.transform.position;
            float distanceToWood = Vector3.Distance(SR04.transform.position, Wood.transform.position);

            // Check if Wood is within 10 degrees in front of SR04
            float angle = Vector3.Angle(SR04.transform.forward, directionToWood);
            if (angle <= 10.0f)
            {
                if (Physics.Raycast(SR04.transform.position, directionToWood, out RaycastHit hit, distanceToWood))
                {
                    if (hit.collider.gameObject == Wood)
                    {
                        // Debug.Log($"SR04 hit something at {distanceToWood:N2}m"); 
                        sensorText.SR04Result = $"SR04 hit something at {distanceToWood:N2}m";
                    }
                }
                else 
                {
                    // Debug.Log("SR04 did not hit anything.");
                    sensorText.SR04Result = "SR04 did not hit anything.";
                }
            }
            else 
                {
                    // Debug.Log("SR04 did not hit anything.");
                    sensorText.SR04Result = "SR04 did not hit anything.";
                }
        }
    }
    public void ToggleAirConditioner(){
        AirConditionerOn = !AirConditionerOn;
        sensorText.AirConditionerStatus = AirConditionerOn;
    }
}