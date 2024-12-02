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
    private Text dayUI { get; set; }
    private Text summaryTextUI { get; set; }
    private Text summaryContentUI { get; set; }
    private Text newDayText { get; set; }

    // Time and Money management (Persistent)
    private bool countTime = true;
    private string timeOfDay { get; set; }
    private float currentTime { get; set; } 
    private int currentDay { get; set; }
    private float endTime { get; set; }      
    private float barMoney { get; set; } 
    private float totalScore { get; set; }

    // Daily bookkeeping
    private float dailyRevenue { get; set; } 
    private float dailyTips { get; set; }
    private float dailyCosts { get; set; } 
    private float dailyTotalScore {get; set;}
    private float dailyNetIncome { get; set; }
    private int dailyCustomersServed { get; set; }
    private bool gameOver { get; set; }
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
        barMoney -= amount;
        dailyCosts += amount;
        dailyNetIncome -= amount;
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
        // If game over
        if(gameOver) {
            currentDay = 1;   // Reset game day to 1
            totalScore = 0f;  // Reset total score
            barMoney = 500.0f; // Reset bar money
            moneyUI.text = "$" + barMoney.ToString("N2").TrimStart('0').TrimStart(',');
            newDayText.text = "Start New Day";         // Reset from 'Restart Game' if needed
        } else {
            currentDay++;
        }
        // Save game data at the end of day (Uses Native SharedPreferences for IOS or Android)
        PlayerPrefs.SetInt("currentDay", currentDay);
        PlayerPrefs.SetFloat("barMoney", barMoney);
        PlayerPrefs.SetFloat("totalScore", totalScore);
        summaryObject.SetActive(false);
        gameOver = false;
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
        barMoney = PlayerPrefs.GetFloat("barMoney", 500f);
        currentDay = PlayerPrefs.GetInt("currentDay", 1);
        totalScore = PlayerPrefs.GetFloat("totalScore", 0f);
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
        moneyUI.text = "$" + barMoney.ToString("N2").TrimStart('0').TrimStart(',');
        summaryTextUI = GameObject.Find("SummaryText").GetComponent<Text>();
        summaryContentUI = GameObject.Find("SummaryContent").GetComponent<Text>();
        newDayText = GameObject.Find("NewDayText").GetComponent<Text>();
        dayUI = GameObject.Find("Day").GetComponent<Text>();
        summaryObject = GameObject.Find("Summary");

        // Disable summaryObject initially
        summaryObject.SetActive(false);

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
            if(barMoney >= 0) {
                dayUI.text = $"Day {currentDay}";
                string netIncomeSign = dailyNetIncome > 0 ? "+" : "-";
                if(dailyNetIncome == 0) {
                    netIncomeSign = "";
                }
                summaryContentUI.text = "Total Customers:\n" +
                                        "-------------------------------------\n" +
                                        "Total Revenue:\n" +
                                        "Cost of Ingredients:\n" +
                                        "Total Tips:\n" +
                                        "-------------------------------------\n" +
                                        "Net Income:\n" + 
                                        "Average Score:\n" +
                                        "Total Score:\n";
                summaryTextUI.text = $"{dailyCustomersServed}\n\n" +
                                    $"+${dailyRevenue:0.00}\n" +
                                    $"-${dailyCosts:0.00}\n" +
                                    $"+${dailyTips:0.00}\n\n" +
                                    $"<b>{netIncomeSign}${dailyNetIncome:0.00}</b>\n" +
                                    $"{getAverageScore()}/100\n" +
                                    $"{totalScore:0.00}\n";
                summaryObject.SetActive(true);
            } 
            // Game lost if money is negative
            else {
                gameOver = true;
                dayUI.text = "Game Over!";
                string totalBarMoney = barMoney.ToString("N2").TrimStart('0').TrimStart(',');
                summaryContentUI.text = "Total Money:\n" +"Total Score:\n";
                summaryTextUI.text = $"{totalBarMoney}\n" + $"{totalScore:0.00}\n";
                newDayText.text = "Restart Game";
                summaryObject.SetActive(true);
            }

        }
        if(countTime) {
            currentTime += Time.deltaTime * 1.25f * 30.0f;      // 1 minute in game time is equivalent to 0.8 seconds in real time.
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
