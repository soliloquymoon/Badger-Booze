using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDrinkManager
{
    private DrinkManager drinkManager;

    [SetUp]
    public void SetUp()
    {
        // Initialize the DrinkManager instance
        drinkManager = new DrinkManager();
    }

    // A Test behaves as an ordinary method
    [Test]
    public void TestPerfectMatchScore()
    {
        // Creating a player drink that matches the Margarita exactly
        Drink playerDrink = new Drink(new Dictionary<string, float>
        {
            {"Tequila", 1.5f}, {"Lime Juice", 1.0f}, {"Triple Sec", 0.5f}
        });

        // Margarita is the first drink in the static list of drinks
        Drink customerOrder = new Drink("Margarita", new Dictionary<string, float>
        {
            {"Tequila", 1.5f}, {"Lime Juice", 1.0f}, {"Triple Sec", 0.5f}
        });

        int score = drinkManager.calculateScore(playerDrink, customerOrder);

        // Expecting a perfect score of 100
        Assert.AreEqual(100, score, "The score should be 100 for a perfect match.");
    }

    [Test]
    public void TestPartialMatchScore()
    {
        // Creating a player drink that partially matches the Bloody Mary (missing Lemon Juice)
        Drink playerDrink = new Drink(new Dictionary<string, float>
        {
            {"Vodka", 1.5f}, {"Tomato Juice", 2.5f}, {"Lemon Juice", 0.3f}
        });

        // Bloody Mary from the static drink list
        Drink customerOrder = new Drink("Bloody Mary", new Dictionary<string, float>
        {
            {"Vodka", 1.5f}, {"Tomato Juice", 3.0f}, {"Lemon Juice", 0.5f}
        });

        int score = drinkManager.calculateScore(playerDrink, customerOrder);

        // The score should be less than 100 but greater than 0 for a partial match
        Assert.Less(score, 100, "The score should be less than 100 for a partial match.");
        Assert.Greater(score, 0, "The score should be greater than 0 for a partial match.");
    }

    [Test]
    public void TestCompleteMismatchScore()
    {
        // Creating a player drink that has completely different ingredients from a Manhattan
        Drink playerDrink = new Drink(new Dictionary<string, float>
        {
            {"Gin", 2.0f}, {"Tonic Water", 4.0f}
        });

        // Manhattan from the static drink list
        Drink customerOrder = new Drink("Manhattan", new Dictionary<string, float>
        {
            {"Rye Whiskey", 2.0f}, {"Sweet Vermouth", 1.0f}, {"Bitters", 0.25f}
        });

        int score = drinkManager.calculateScore(playerDrink, customerOrder);

        // Expecting a score of 0 for a complete mismatch
        Assert.AreEqual(0, score, "The score should be 0 for a complete mismatch.");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestDrinkManagerWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
