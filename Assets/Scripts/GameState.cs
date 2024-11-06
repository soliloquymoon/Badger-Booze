using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    private Text clockUI;
    private Text moneyUI { get; set; }
    private string timeOfDay { get; set; }
    private float currentTime { get; set; } 
    private float endTime { get; set; }      
    private float barMoney { get; set; } 
    private bool countTime = false;

    // Gets called by DrinksManager every time a drink score is calculated
    public void AddMoney(float amount) {
        if(barMoney + amount <= 999999f) {
            barMoney += amount;
        } else {
            barMoney = 999999f;
        }
        moneyUI.text = "$" + barMoney.ToString("000,000");
    }

    // Use to start or resume game time.
    public void StartTimer() {
        countTime = true;
    }

    // Use to pause game time.
    public void StopTimer() {
        countTime = false;
    }

    void Start()
    {
        // Set up time and money variables
        currentTime = 480.0f; // Starts at 8:00, equivalent to 480 minutes
        endTime = 1440.0f;  // Ends at midnight, equivalent to 1440 minutes
        timeOfDay = "AM";
        barMoney = 0.0f;    // TODO: Retrieve from save instead (Save per day?)

        // Get UI components
        clockUI = GameObject.Find("TimeText").GetComponent<Text>();
        clockUI.text = "08:00 AM";
        moneyUI = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyUI.text = "$" + barMoney.ToString("000,000");

        // Rebuild all UI Layout on Start to Prevent UI glitches
        LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("TopLeft").GetComponent<RectTransform>());
    }

    void Update()
    {
        if(currentTime >= endTime) {
            countTime = false;
        }
        if(countTime) {
            currentTime += Time.deltaTime * 1.25f;      // 1 minute in game time is equivalent to 0.8 seconds in real time.
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
    }
}
