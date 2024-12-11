using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    // List to store the unlocked ingredients
    private Dictionary<string, Ingredient> ingredientObjects;
    private DrinkManager drinkManager;

    /*
     * OnEnable: Called everytime IngredientManager object is activated.
     * It saves the new ingredient objects into a dictionary and set them active
     * or inactive according to the unlocked recipe list.
     */
    void OnEnable()
    {
        ingredientObjects = new Dictionary<string, Ingredient>();
        foreach (Ingredient obj in FindObjectsOfType<Ingredient>(true))
        {
            ingredientObjects.Add(obj.name, obj);
            obj.gameObject.SetActive(false);
        }
        drinkManager = GameObject.Find("DrinksManager").GetComponent<DrinkManager>();
        for (int i = 0; i < drinkManager.GetUnlockedDrinkNames().Count; i++)
        {
            UnlockNewIngredients(drinkManager.getDrinkList()[i]);
        }
    }

    /*
     * UnlockNewIngredients: Unlock new ingredients based on the ingredients in a given Drink object.
     */
    public void UnlockNewIngredients(Drink drink)
    {
        // New ingredient is unlocked if any ingredient of the drink is not unlocked yet
        foreach (string ingredientName in drink.getIngredients().Keys)
        {
            ingredientObjects[ingredientName].gameObject.SetActive(true);
        }
    }
}