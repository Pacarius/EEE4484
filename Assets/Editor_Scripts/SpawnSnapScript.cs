using UnityEngine;
using System.Collections;

public class SpawnSnapScript : MonoBehaviour 
{
    public GameObject snapPrefab;
    public void SpawnSnaps(){
        if(snapPrefab != null) foreach(Transform child in transform){
            Instantiate(snapPrefab, child);
        }
    }
}