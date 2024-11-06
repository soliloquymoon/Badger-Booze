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

    /*
     * Used for creating pre-determined drinks for the drinkList (internal use only)
     */
    public Drink(string name, Dictionary<string, float> ingredients) {
        this.name = name;
        this.ingredients = ingredients;
    }

    /*
     * Used for creating Drink objects served by the player, no need for a drink name
     */
    public Drink(Dictionary<string, float> ingredients) {
        this.ingredients = ingredients;
    }

    public string getName() {
        return name;
    }

    public Dictionary<string, float> getIngredients() {
        return ingredients;
    }
}
