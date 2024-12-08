using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    private Dictionary<string, Sprite> ingredientSprites;
    private GameObject liquorItems;
    private GameObject juiceItems;
    private List<string> ingredientItems;
    
    void Start()
    {
        ingredientSprites = new Dictionary<string, Sprite>();
        liquorItems = GameObject.Find("LiquorItems");
        juiceItems = GameObject.Find("JuiceItems");
        LoadSprites();
        ingredientItems = new List<string>{"Tequila", "Simple Syrup", "Lime Juice"};
    }

    void LoadSprites()
    {
        Sprite[] juiceSprites = Resources.LoadAll<Sprite>("Juice");
        Sprite[] liquorSprites = Resources.LoadAll<Sprite>("Liquor");
        foreach (var sprite in juiceSprites)
            ingredientSprites[sprite.name.Replace("_", " ")] = sprite;
        foreach (var sprite in liquorSprites)
            ingredientSprites[sprite.name.Replace("_", " ")] = sprite;
    }

    public void UnlockNewIngredients(Drink drink)
    {
        Debug.Log("New drink: " + drink.getName());
        foreach (var ingredientName in drink.getIngredients().Keys)
        {
            if (!ingredientItems.Contains(ingredientName))
            {
                Debug.Log("New ingredient: " + ingredientName);
                ingredientItems.Add(ingredientName);
                GameObject newIngredient;
                if (ingredientName.Contains("Juice"))
                    newIngredient = Instantiate(juiceItems.transform.GetChild(1), juiceItems.transform).gameObject;
                else
                    newIngredient = Instantiate(liquorItems.transform.GetChild(0), liquorItems.transform).gameObject;

                newIngredient.GetComponent<Ingredient>().SetName(ingredientName);
                Debug.Log(ingredientSprites.ContainsKey(ingredientName));
                newIngredient.GetComponent<Ingredient>().SetSprite(ingredientSprites[ingredientName]);
            }
        }
    }
}