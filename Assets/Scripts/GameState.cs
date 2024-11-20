using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
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
    private bool countTime = true;
    private GameObject currentCustomer;

    // Gets called by DrinksManager every time a drink score is calculated
    public void AddMoney(float amount) {
        if(barMoney + amount <= 999999f) {
            barMoney += amount;
        } else {
            barMoney = 999999f;
        }
        moneyUI.text = "$" + barMoney.ToString("###,###");
    }

    // Called when ingredients are used or new ingredients are unlocked.
    public void DeductMoney(float amount) {
        if(barMoney - amount < 0f) {
            throw new Exception("Not enough money.");
        } else {
            barMoney -= amount;
        }
        moneyUI.text = "$" + barMoney.ToString("###,###");
    }

    // Use to start or resume game time.
    public void StartTimer() {
        countTime = true;
    }

    // Use to pause game time.
    public void StopTimer() {
        countTime = false;
    }

    public void NewDay() {
        currentCustomer.GetComponent<Customer>().NewCustomer();
        currentCustomer.SetActive(true);
        currentTime = 1080.0f;
        timeOfDay = "PM";
        countTime = true;
    }

    public bool isDayDone() {
        return !countTime;
    }

    void Start()
    {
        // Set up time and money variables
        currentTime = 1080.0f; // Starts at 6PM, equivalent to 1080 minutes
        endTime = 1560.0f;  // Ends at 2AM, equivalent to 1560 minutes
        timeOfDay = "PM";
        barMoney = 500.0f;    // TODO: Retrieve from save instead (Save per day?)

        // Get UI components
        clockUI = GameObject.Find("TimeText").GetComponent<Text>();
        clockUI.text = "08:00 AM";
        moneyUI = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyUI.text = "$" + barMoney.ToString("###,###");

        // Get Customer
        currentCustomer = GameObject.Find("Customer");

        // Rebuild all UI Layout on Start to Prevent UI glitches
        LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("TopLeft").GetComponent<RectTransform>());
    }

    void Update()
    {
        if(currentTime >= endTime) {
            countTime = false;
            currentCustomer.SetActive(false);
            // TODO: Implement end of day, save game settings and time using PlayerPrefs
        }
        if(countTime) {
            currentTime += Time.deltaTime * 1.25f;      // 1 minute in game time is equivalent to 0.8 seconds in real time.
            int hour = Mathf.FloorToInt((currentTime / 60.0f));
            // Convert to 12-hour format
            if(hour != 12) {
                hour %= 12;
            }
            // Switch to AM after midnight
            if(hour == 0) {
                timeOfDay = "AM";
            }
            int minute = Mathf.FloorToInt(currentTime % 60.0f);
            clockUI.text = hour.ToString("00") + ":" + minute.ToString("00") + " " + timeOfDay;
        }
    }
}
