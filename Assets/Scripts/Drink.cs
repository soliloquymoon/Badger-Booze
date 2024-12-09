using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to represent a specific cocktail on the drinkList, or a drink made by the player.
 */
public class Drink {
    private string name { get; set; }
    // Maps each ingredient name to its amount (represented as a float)
    private Dictionary<string, float> ingredients { get; set; }
    // Total amount of all ingredients added
    private float totalAmount;
    private float maxAmount = 10f;
    private float minAmount = 2f;

    /*
     * Used for creating pre-determined drinks for the drinkList (internal use only)
     */
    public Drink(string name, Dictionary<string, float> ingredients) {
        this.name = name;
        this.ingredients = ingredients;
    }

    /*
     * Used for creating Drink objects served by the player, starting with empty dictionary
     */
    public Drink() {
        this.ingredients = new Dictionary<string, float>();
        this.totalAmount = 0f;
    }

    public string getName() {
        return name;
    }

    public Dictionary<string, float> getIngredients() {
        return ingredients;
    }

    /*
     * AddIngredient: Adds an ingredient to the ingredients dictionary if not present,
     * updates its quantity incremented by 0.0001f otherwise.
     */
    public void AddIngredient(string ingredientName) {
        // Adds ingredient only if it is less than or equal to the maximum amount (=10f)
        if (totalAmount <= maxAmount)
        {
            if (ingredients.ContainsKey(ingredientName))
            {
                totalAmount += 0.0001f;
                ingredients[ingredientName] += 0.0001f;
            }
            else
            {
                totalAmount += 0.0001f;
                ingredients.Add(ingredientName, 0.0001f);
            }
        }

    }

    /*
     * Reset: Resets the ingredients dictionary to an empty state.
     * This method is called when a new customer walks in.
     */
    public void Reset() {
        totalAmount = 0f;
        this.ingredients = new Dictionary<string, float>();
    }

    /*
     * CheckMinBound: Checks if the total amount of ingredients exceed
     * the minimum required amount (=2f) or not.
     */
    public bool CheckMinBound()
    {
        return this.totalAmount >= minAmount;
    }

    /*
     * CheckMaxBound(): Checks if the total amount of ingredients exceed
     * the maximum required amount (=10f) or not.
     */
    public bool CheckMaxBound()
    {
        return this.totalAmount <= maxAmount;
    }
}
