using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    private Text clockUI;
    private Text moneyUI;
    private float currentTime = 480.0f; // Starts at 8:00, equivalent to 480 minutes
    private string timeOfDay = "AM";
    private float endTime = 1440.0f;  // Ends at midnight, equivalent to 1440 minutes
    private float barMoney = 0.0f; // TODO: Retrieve from save instead (Save per day?)

    // Use to start, pause, or resume game time.
    public bool countTime = true;

    void Start()
    {
        clockUI = GameObject.Find("TimeText").GetComponent<Text>();
        moneyUI = GameObject.Find("MoneyText").GetComponent<Text>();
    }

    void Update()
    {
        if(currentTime >= endTime) {
            countTime = false;
        }
        if(countTime) {
            currentTime += Time.deltaTime * 2.0f;      // 1 minute in game time is equivalent to 0.5 seconds in real time.
            int hour = Mathf.FloorToInt((currentTime / 60.0f));
            // Convert to 12-hour format
            if(hour != 12) {
                hour %= 12;
            }
            if(hour >= 12 && hour < 24) {
                timeOfDay = "PM";
            }
            int minute = Mathf.FloorToInt(currentTime % 60.0f);
            clockUI.text = hour.ToString("00") + ":" + minute.ToString("00") + " " + timeOfDay;
        }
        // TODO: Update money based on order rating
    }
}
