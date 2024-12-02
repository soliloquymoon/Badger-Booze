using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    // UI Text Objects
    private Text clockUI { get; set;}
    private Text moneyUI { get; set; }
    private Text dayUI { get; set;}
    private Text summaryUI { get; set;}

    // Time and Money management (Persistent)
    private bool countTime = true;
    private string timeOfDay { get; set; }
    private float currentTime { get; set; } 
    private int currentDay { get; set; }
    private float endTime { get; set; }      
    private float barMoney { get; set; } 

    // Daily bookkeeping
    public float dailyRevenue { get; set; } 
    public float dailyTips { get; set; }
    public float dailyCosts { get; set; } 
    public float dailyTotalScore {get; set;}
    private float dailyNetIncome { get; set; }
    private int dailyCustomersServed { get; set; }
    private GameObject currentCustomer;
    private GameObject summaryObject;

    // Gets called by DrinksManager every time a drink score is calculated
    public void AddMoney(float amount) {
        if(barMoney + amount <= 999999f) {
            barMoney += amount;
            dailyRevenue += amount;
            dailyNetIncome += amount;
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
            dailyCosts += amount;
            dailyNetIncome -= amount;
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
        dailyCosts = 0f;
        dailyCustomersServed = 0;
        dailyRevenue = 0f;
        dailyTips = 0f;
        dailyNetIncome = 0f;
        dailyTotalScore = 0f;
        currentDay++;
        summaryObject.SetActive(false);
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
        barMoney = PlayerPrefs.GetFloat("barMoney", 500.0f);
        currentDay = PlayerPrefs.GetInt("currentDay", 1);
        dailyCosts = 0f;
        dailyCustomersServed = 0;
        dailyRevenue = 0f;
        dailyTips = 0f;
        dailyNetIncome = 0f;
        dailyTotalScore = 0f;

        // Get UI components
        clockUI = GameObject.Find("TimeText").GetComponent<Text>();
        clockUI.text = getTimeString();
        moneyUI = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyUI.text = "$" + barMoney.ToString("###,###");
        summaryUI = GameObject.Find("SummaryText").GetComponent<Text>();
        dayUI = GameObject.Find("Day").GetComponent<Text>();
        summaryObject = GameObject.Find("Summary");
        summaryObject.SetActive(false);
        string netIncomeSign = dailyNetIncome > 0 ? "+" : "-";
        if(dailyNetIncome == 0) {
            netIncomeSign = "";
        }
        summaryUI.text = $@"{dailyCustomersServed}
        
                                +${dailyRevenue}
                                -${dailyCosts}
                                +${dailyTips}

                                <b>{netIncomeSign}${dailyNetIncome}</b>
                                {getAverageScore()}/100";
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
            // Display end of day summary
            dayUI.text = $"Day {currentDay}";
            string netIncomeSign = dailyNetIncome > 0 ? "+" : "-";
            if(dailyNetIncome == 0) {
                netIncomeSign = "";
            }
            summaryUI.text = $@"{dailyCustomersServed}
        
                                    +${dailyRevenue}
                                    -${dailyCosts}
                                    +${dailyTips}

                                    <b>{netIncomeSign}${dailyNetIncome}</b>
                                    {getAverageScore()}/100";
            summaryObject.SetActive(true);
            
            // Update daily values
            dailyCosts = 0f;
            dailyCustomersServed = 0;
            dailyRevenue = 0f;
            dailyTips = 0f;
            dailyNetIncome = 0f;
            dailyTotalScore = 0f;
            // Save game data at the end of day (Uses Native SharedPreferences for IOS or Android)
            PlayerPrefs.SetInt("currentDay", currentDay);
            PlayerPrefs.SetFloat("barMoney", barMoney);
        }
        if(countTime) {
            currentTime += Time.deltaTime * 1.25f;      // 1 minute in game time is equivalent to 0.8 seconds in real time.
            clockUI.text = getTimeString();
        }
    }

    string getTimeString() {
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
        return hour.ToString("00") + ":" + minute.ToString("00") + " " + timeOfDay;
    }

    int getAverageScore() {
        return dailyCustomersServed > 0
            ? (int)Math.Round(Math.Clamp(dailyTotalScore / dailyCustomersServed, 0, 100))
            : 0;
    }
}
