using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine;

public class WristCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public Transform wristBone;
    [Range(20, 60)]
    public int activationAngle = 45;
    public GameObject wristCanvas;
    [SerializeField]
    private OVRHan hand;
    private Pose currentPose;
    private HandJointId handJointId = HandJointId.HandWristRoot; // TO DO: Change this to your bone.
    void Start()
    {
        wristCanvas.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hand.GetJointPose(handJointId, out currentPose);
        if (currentPose == null|| !wristCanvas) return;
        Vector3 CameraDirection = Vector3.Normalize(Camera.main.transform.position - currentPose.position);
        Vector3 PalmNormal = currentPose.forward;
        float CameraAngle = Vector3.Angle(PalmNormal, CameraDirection);
        if (CameraAngle <= activationAngle) wristCanvas.SetActive(true);
        else wristCanvas.SetActive(false);
    }
}
