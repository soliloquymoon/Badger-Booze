using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Receipt : MonoBehaviour
{
    Dictionary<string, Text> texts;
    Text ingredientText;

    void Start()
    {
        ingredientText = this.transform.GetChild(0).GetComponent<Text>();
        ingredientText.text = "";
        texts = new Dictionary<string, Text>();
    }

    public void SetText(Drink drink, string ingredientName) {
        if (ingredientText.text.Equals("")) {
            texts.Add(ingredientName, ingredientText);
        }
        if (!texts.ContainsKey(ingredientName)) {
            texts.Add(ingredientName, GameObject.Instantiate(ingredientText, this.transform));
            Debug.Log("New ingredient:" + ingredientName);
        } else {
            texts[ingredientName].text = ingredientName + " --- " +
            ConvertToMl(drink.getIngredients()[ingredientName]) + "ml";
        }
    }

    string ConvertToMl(float amount) {
        return (amount * 30).ToString("F1");
    }

    public void Reset() {
        int index = 0;
        foreach (Transform child in this.transform) {
            if (index > 0)
                Destroy(child.gameObject);
        }
        texts = new Dictionary<string, Text>();
        ingredientText.text = "";
    }
}
