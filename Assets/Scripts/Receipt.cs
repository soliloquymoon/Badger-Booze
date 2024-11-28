using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Receipt : MonoBehaviour
{
    // A dictionary to store ingredient names and their corresponding Text objects.
    Dictionary<string, Text> texts;
    // A reference to the UI Text component used to display ingredient amounts.
    Text ingredientText;

    void Start()
    {
        ingredientText = this.transform.GetChild(0).GetComponent<Text>();
        ingredientText.text = "";
        texts = new Dictionary<string, Text>();
    }

    /*
     * SetText: Updates a Text object to display information about an ingredient.
     * If the ingredient is initially added to the shaker, a new Text object is created.
     * Parameters:
     *   - drink: Drink object containing ingredients.
     *   - ingredientName: Name of the ingredient added.
     */
    public void SetText(Drink drink, string ingredientName) {
        // If ingredientText is currently empty, it is the first ingredient to be added.
        if (ingredientText.text.Equals("")) {
            texts.Add(ingredientName, ingredientText);
        }
        // If the ingredient doesn't exist in the dictionary, create a new Text object for it.
        if (!texts.ContainsKey(ingredientName)) {
            // Instantiate a new Text object and add it as a child of the Receipt GameObject.
            texts.Add(ingredientName, GameObject.Instantiate(ingredientText, this.transform));
        } else {
            // Update the text of the existing Text object for the ingredient.
            texts[ingredientName].text = ingredientName + " --- " +
            drink.getIngredients()[ingredientName].ToString("F1") + "oz";
        }
    }

    /*
     * Reset: Resets the receipt by removing all the ingredient Text entries.
     * Clears the texts dictionary and resets the first ingredient text field.
     */
    public void Reset() {
        int index = 0;
        // Loop through all the child objects under this Receipt object.
        foreach (Transform child in this.transform) {
            // Skip the first child (ingredientText) and destroy all other children.
            if (index > 0)
                Destroy(child.gameObject);
        }
        texts = new Dictionary<string, Text>();
        ingredientText.text = "";
    }
}
