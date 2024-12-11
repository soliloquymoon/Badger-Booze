using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
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
    public bool countTime = true;
    public float gameSpeed = 3.0f;
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
    private GameObject customerObject;
    private GameObject summaryObject;
    private Customer customer;
    private DrinkManager drinkManager;
    private ReactionManager reactionManager;
    // Scene Management
    private GameObject bartendingScene;

    public void DrinkServed() {
        Drink userDrink = customer.GetMixingDrink();
        Drink customerOrder = customer.GetOrderedDrink();
        int score = drinkManager.calculateScore(userDrink, customerOrder);
        // A drink score of 80 earns $8, a score of 70 earns $7 and so on...
        float money = (float) score / 10f;
        // Mood score determines the percentage of tip you receive
        // Example: 80% mood and a score of 85 means you get 8.5 * 80/100 = $15.3
        float tip = money * (customer.GetMoodScore() / 100);

        // Update scores
        dailyTotalScore += score;
        totalScore += score;

        // Update money
        AddMoney(money + tip);
        dailyTips += tip;

        // Increment customers served
        dailyCustomersServed++;

        // Display customer reaction
        string orderReaction = reactionManager.GetReaction(score);
        customer.DisableDialogueButtons();
        customer.SetDialogueText(orderReaction);
        
        //play reaction dialogue sfx of appropriate length
        System.Random rnd = new System.Random();
        int dialogue;

        //checks whether the reaction text length is short or long to play the appropriate audio length
        if(orderReaction.Length < 40) {
            dialogue = rnd.Next(5);
                    
            switch (dialogue) {
                case 0:
                    AudioManager.Instance.PlaySFX("ShortDialogueA");
                    break;
                case 1:
                    AudioManager.Instance.PlaySFX("ShortDialogueB");
                    break;
                case 2:
                    AudioManager.Instance.PlaySFX("ShortDialogueC");
                    break;
                case 3:
                    AudioManager.Instance.PlaySFX("ShortDialogueD");
                    break;
                case 4:
                    AudioManager.Instance.PlaySFX("ShortDialogueE");
                    break;
            }
        } else {
            dialogue = rnd.Next(4);
                    
            switch (dialogue) {
                case 0:
                    AudioManager.Instance.PlaySFX("LongDialogueA");
                    break;
                case 1:
                    AudioManager.Instance.PlaySFX("LongDialogueB");
                    break;
                case 2:
                    AudioManager.Instance.PlaySFX("LongDialogueC");
                    break;
                case 3:
                    AudioManager.Instance.PlaySFX("LongDialogueD");
                    break;
            }
        }

        StartCoroutine(waitForReaction());
    }

    IEnumerator waitForReaction() {
        yield return new WaitForSeconds(3f);
        GameObject.Find("Cocktail").transform.SetParent(bartendingScene.transform);
        customer.NewCustomer();
        customer.EnableDialogueButtons();
        // Reset Bartending Scene
        Destroy(bartendingScene);
        bartendingScene = Instantiate(Resources.Load<GameObject>("BartendingScene"), GameObject.Find("Canvas").transform); 
        bartendingScene.transform.SetSiblingIndex(0);
        bartendingScene.name = "BartendingScene"; // Gets rid of the "(Clone)" label
    }

    public void AddMoney(float amount) {
        if(barMoney + amount <= 999999f) {
            barMoney += amount;
            dailyRevenue += amount;
            dailyNetIncome += amount;
        } else {
            barMoney = 999999f;
        }
        moneyUI.text = "$" + barMoney.ToString("N2").TrimStart('0').TrimStart(',');
    }

    // Called when ingredients are used or new ingredients are unlocked.
    public void DeductMoney(float amount) {
        barMoney -= amount;
        dailyCosts += amount;
        dailyNetIncome -= amount;
        moneyUI.text = "$" + barMoney.ToString("N2").TrimStart('0').TrimStart(',');
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
        customer.NewCustomer();
        customerObject.transform.SetSiblingIndex(2); // Put customer in-front of bar again to display it
        currentTime = 1080.0f;
        timeOfDay = "PM";
        dailyCosts = 0f;
        dailyCustomersServed = 0;
        dailyRevenue = 0f;
        dailyTips = 0f;
        dailyNetIncome = 0f;
        dailyTotalScore = 0f;
        drinkManager.UnlockNewDrink(barMoney);

        // If game over
        if(gameOver) {
            currentDay = 1;   // Reset game day to 1
            totalScore = 0f;  // Reset total score
            barMoney = 50.0f; // Reset bar money
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
        barMoney = PlayerPrefs.GetFloat("barMoney", 50f);
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

        // Get Initial Bartending Scene
        bartendingScene = GameObject.Find("BartendingScene");

        // Disable summaryObject initially
        summaryObject.SetActive(false);

        // Get Customer and Drink Manager
        customerObject = GameObject.Find("Customer");
        customer = customerObject.GetComponent<Customer>();
        reactionManager = GameObject.Find("ReactionManager").GetComponent<ReactionManager>();
        drinkManager = GameObject.Find("DrinksManager").GetComponent<DrinkManager>();

        // Rebuild all UI Layout on Start to Prevent UI glitches
        LayoutRebuilder.ForceRebuildLayoutImmediate(GameObject.Find("TopLeft").GetComponent<RectTransform>());
    }

    void Update()
    {
        if(currentTime >= endTime) {
            countTime = false;
            // Hide customer by re-ordering it behind the bar
            customerObject.transform.SetSiblingIndex(0);
            // Display end of day summary
            if(barMoney >= 0) {
                dayUI.text = $"Day {currentDay}";
                string netIncomeSign = dailyNetIncome > 0 ? "+" : "-";
                if(dailyNetIncome == 0) {
                    netIncomeSign = "";
                }
                dailyNetIncome = Mathf.Abs(dailyNetIncome); // Sign is already determined, only show value now
                summaryContentUI.text = "Total Customers:\n" +
                                        "------------------------------------\n" +
                                        "Total Revenue:\n" +
                                        "Cost of Ingredients:\n" +
                                        "Total Tips:\n" +
                                        "------------------------------------\n" +
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
                // Reset Bartending Scene
                Destroy(bartendingScene);
                bartendingScene = Instantiate(Resources.Load<GameObject>("BartendingScene"), GameObject.Find("Canvas").transform); 
                bartendingScene.transform.SetSiblingIndex(0);
                bartendingScene.name = "BartendingScene"; // Gets rid of the "(Clone)" label
                GameObject.Find("Closed Shaker").GetComponent<ShakeDetector>().ResetBartendingScene();
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
            currentTime += Time.deltaTime * gameSpeed;      // Scale game time
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
