using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour{
    public GameObject Esp32;
    public GameObject Esp64;
 private static StateManager _singleton;
    public RecolorHoles _holes;
    public GameObject DHT11;
    public GameObject SR04;
        public static StateManager Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(StateManager)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }
    void Awake()
    {
        Singleton = this;    
    }
    public void SetEsp32(Vector3 vector)
    {
        //Debug.Log(Esp32.name);
        Esp32.transform.localPosition = vector;
        Esp32.transform.rotation = Quaternion.identity;
        _holes.Color();
        Esp64.SetActive(false);
    }
    public void SetDHT11(Vector3 vector)
    {
        DHT11.transform.localPosition = vector;
        DHT11.transform.rotation = Quaternion.identity;
    }
    public void setSR04(Vector3 vector)
    {
        SR04.transform.localPosition = vector;
        SR04.transform.rotation = Quaternion.identity;
    }
}