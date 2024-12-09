using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class DrinkManager : MonoBehaviour {
    
    /*
     * Static list of all possible orders in the game
     */
    private static List<Drink> drinkList = new List<Drink>
    {
        new Drink("Margarita", new Dictionary<string, float> 
        { 
            {"Tequila", 1.5f}, {"Lime Juice", 1.0f}, {"Triple Sec", 0.5f}
        }),
        
        new Drink("Bloody Mary", new Dictionary<string, float> 
        { 
            {"Vodka", 1.5f}, {"Tomato Juice", 3.0f}, {"Lemon Juice", 0.5f}
        }),
        
        new Drink("Cosmopolitan", new Dictionary<string, float> 
        { 
            {"Vodka", 1.5f}, {"Lime Juice", 0.5f}, {"Cranberry Juice", 1.0f}, {"Cointreau", 0.5f}
        }),

        new Drink("Vesper", new Dictionary<string, float> 
        { 
            {"Gin", 1.5f}, {"Vodka", 0.5f}, {"Lillet Blanc", 0.25f}
        }),

        new Drink("Whiskey Sour", new Dictionary<string, float> 
        { 
            {"Whiskey", 2.0f}, {"Lemon Juice", 0.75f}, {"Simple Syrup", 0.5f}
        }),

        new Drink("Paradise", new Dictionary<string, float> 
        { 
            {"Gin", 2.0f}, {"Brandy", 1.5f}, {"Orange Juice", 1.0f}
        }),

        new Drink("French 75", new Dictionary<string, float> 
        { 
            {"Gin", 2.0f}, {"Lemon Juice", 1.0f}, {"Simple Syrup", 0.5f}, {"Champagne", 4.0f}
        }),

        new Drink("White Lady", new Dictionary<string, float> 
        { 
            {"Gin", 2.5f}, {"Triple Sec", 2.0f}, {"Lemon Juice", 1.75f}
        }),

        new Drink("Negroni", new Dictionary<string, float> 
        { 
            {"Gin", 1.0f}, {"Campari", 1.0f}, {"Sweet Vermouth", 1.0f}
        }),

        new Drink("New York Sour", new Dictionary<string, float> 
        { 
            {"Whiskey", 4.0f}, {"Lemon Juice", 2.0f}, {"Simple Syrup", 1.5f}, {"Red Wine", 0.5f}
        })
    };

    private List<string> unlockedDrinks = new List<string>();
    private int nextDrinkIndex;
    public IngredientManager ingredientManager;

    void Awake()
    {
        nextDrinkIndex = 0;
        UnlockNewDrink(50);
    }

    public List<Drink> getDrinkList() {
        return drinkList;
    }

    public Drink getDrink(string name) {
        foreach (Drink drink in drinkList) {
            if (drink.getName() == name) {
                return drink;
            }
        }
        return null;
    }

    public void UnlockNewDrink(float earnings)
    {
        Debug.Log(earnings);
        if (earnings / 100 >= nextDrinkIndex)
            unlockedDrinks.Add(drinkList[nextDrinkIndex].getName());
            ingredientManager.UnlockNewIngredients(drinkList[nextDrinkIndex]);
            nextDrinkIndex++;
    }

    public List<string> GetUnlockedDrinkNames()
    {
        return this.unlockedDrinks;
    }

    /*
     * Calculates the difference between the customer's order and the player's drink, returns a score.
     * playerDrink has no name, and is not on the drinkList.
     * customerOrder always has a name, and is on the drinkList.
     */
    public int calculateScore(Drink playerDrink, Drink customerOrder) {
        float totalDifference = 0.0f;
        float maxDifference = 0.0f;

        Dictionary<string, float> playerIngredients = playerDrink.getIngredients();
        Dictionary<string, float> orderIngredients = customerOrder.getIngredients();

        foreach (KeyValuePair<string, float> ingredient in orderIngredients) {
            maxDifference += ingredient.Value; // Calculate max possible difference (missing all ingredients)

            if (playerIngredients.ContainsKey(ingredient.Key)) {
                // Ingredient exists, add quantity difference to total difference
                totalDifference += Mathf.Abs(playerIngredients[ingredient.Key] - ingredient.Value);
            } else {
                // Ingredient missing, add whole quantity to total difference
                totalDifference += ingredient.Value;
            }
        }

        // Calculate the difference percentage
        float differenceRatio = totalDifference / maxDifference;

        // Invert the ratio to get the closeness percentage instead of the difference percentage
        int closeness = (int)(100 * (1 - differenceRatio));
        // Ensure the score is between 0 and 100
        int score = Mathf.Clamp(closeness, 0, 100);
        return score;
    }
}