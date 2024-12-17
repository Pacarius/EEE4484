using System.Collections;
using TMPro;
using UnityEngine;

public class SensorText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string SR04Result;
    public bool AirConditionerStatus;
    System.Random System = new();
    string airConditionerStatus {get {return AirConditionerStatus ? "On" : "Off";}}
    double temp = 0f;
    double humidity = 0;
    Coroutine textUpdate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        textUpdate = StartCoroutine(updateText());
    }
    void FixedUpdate(){
        temp = System.NextDouble() * 5 + (AirConditionerStatus ? 18 : 25);
        humidity =  (AirConditionerStatus ? 0.3 : 0.5) + (System.NextDouble() * 0.4);
    }
    IEnumerator updateText(){
        while(true){
            string t = $"Well, the project's all done! Now, you can test out the sensors. \nAir Conditioner is {airConditionerStatus} \nSR04: {SR04Result}\nAir Conditioner: {airConditionerStatus}\nTemperature: {temp:N2}°C\nHumidity: {humidity* 100:N2}%";
            text.text = t;
            yield return new WaitForSeconds(1);
        }
    }
    // Update is called once per frame
    void OnDisable()
    {
        StopCoroutine(textUpdate);
    }
}
