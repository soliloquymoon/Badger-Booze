using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    // Reference to the Transform components
    private Transform liquorItems;
    private Transform juiceItems;

    // List to store the unlocked ingredients
    private List<string> unlockedItems = new List<string>();
    
    void Start()
    {
        // Find the "LiquorItems" and "JuiceItems" GameObjects in the scene by name and assign their Transforms
        this.liquorItems = GameObject.Find("LiquorItems").transform;
        this.juiceItems = GameObject.Find("JuiceItems").transform;
        
        // Show the ingredient items based on the unlocked items list
        ShowIngredientItems();
    }
    
    /*
     * UnlockNewIngredients: Unlock new ingredients based on the ingredients in a given Drink object.
     */
    public void UnlockNewIngredients(Drink drink)
    {
        // New ingredient is unlocked if any ingredient of the drink is not unlocked yet
        foreach (var ingredientName in drink.getIngredients().Keys)
        {
            if (!unlockedItems.Contains(ingredientName))
                unlockedItems.Add(ingredientName);                
        }


        // Ensure the game objects are updated on Awake()
        this.liquorItems = GameObject.Find("LiquorItems").transform;
        this.juiceItems = GameObject.Find("JuiceItems").transform;

        // Save the updated unlocked ingredients list to PlayerPrefs
        SaveUnlockedItems();
        ShowIngredientItems();
    }

    /*
     * ShowIngredientItems: Display the ingredient items in the scrollbar UI.
     */
    private void ShowIngredientItems()
    {
        LoadUnlockedItems();
        for (int i = 0; i < liquorItems.childCount; i++)
        {
            GameObject child = liquorItems.GetChild(i).gameObject;
            if (unlockedItems.Contains(child.name))
                child.SetActive(true);
            else
                child.SetActive(false);
        }
        for (int i = 0; i < juiceItems.childCount; i++)
        {
            GameObject child = juiceItems.GetChild(i).gameObject;
            if (unlockedItems.Contains(child.name))
                child.SetActive(true);
            else
                child.SetActive(false);
        }
    }

    /*
     * SaveUnlockedItems: Save the unlocked ingredients to PlayerPrefs.
     */
    private void SaveUnlockedItems()
    {
        string unlockedItemsString = string.Join(",", unlockedItems);
        PlayerPrefs.SetString("UnlockedItems", unlockedItemsString);
        PlayerPrefs.Save();
    }

    /*
     * LoadUnlockedItems: Load the unlocked ingredients from PlayerPrefs.
     */
    private void LoadUnlockedItems()
    {
        if (PlayerPrefs.HasKey("UnlockedItems"))
        {
            string unlockedItemsString = PlayerPrefs.GetString("UnlockedItems");
            unlockedItems = unlockedItemsString.Split(',').ToList();
        }
    }
}