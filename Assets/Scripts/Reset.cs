using System.Collections;
using UnityEngine;

public class Reset : MonoBehaviour{
	void Awake(){
		StartCoroutine(ResetScene());
	}
    IEnumerator ResetScene()
    {
        yield return new WaitForSeconds(0.1f);
        OVRManager.display.RecenterPose();
    }
}
