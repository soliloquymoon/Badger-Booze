using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to represent a specific cocktail on the drinkList, or a drink made by the player.
 */
public class Order {
    private string drinkName { get; set; }
    private string customerOrder { get; set; }

    /*
     * Used for creating pre-determined drinks for the drinkList (internal use only)
     */
    public Order(string drinkName, string customerOrder) {
        this.drinkName = drinkName;
        this.customerOrder = customerOrder;
    }

    public string getCustomerOrder() {
        return this.customerOrder;
    }

    public string getDrinkName() {
        return this.drinkName;
    }

    public void setDrinkName(string drinkName) {
        this.drinkName = drinkName;
        this.customerOrder = this.customerOrder.Replace("*", drinkName);
    }
}


public class OrderManager {
    /*
     * Static list of all possible orders in the game
     */
    private Dictionary<string, List<Order>> orderDict = new Dictionary<string, List<Order>>
    {
        // Random order (replace * with drink name)
        { "", new List<Order> {
            new Order("", "Can I have a *?"),
            new Order("", "Make my *, please!"),
            new Order("", "I’d like a *."),
            new Order("", "I want a *, please."),
            new Order("", "One *!"),
            new Order("", "I need a wonderful * for a wonderful night!"),
            new Order("", "How about *?"),
            new Order("", "Can you make a *?"),
            new Order("", "I'm in the mood for *."),
            new Order("", "I want to dance with a *."),
            new Order("", "I’d like a * to help me process my life choices."),
            new Order("", "I think a * would be perfect for celebrating my survival of another week!"),
            new Order("", "Just one *—I’m on a strict ‘one-drink’ diet... for now!"),
            new Order("", "I think a * will help me study... or at least help me pretend to!"),
            new Order("", "I think a * will help me network..."),
            new Order("", "A * would really help me forget that group project that’s due tomorrow!"),
            new Order("", "What’s the most Instagrammable * you have? I need likes, not just drinks!"),
            new Order("", "Can I get a *? It’s essential for my ‘surviving finals’ kit!"),
            new Order("", "One *, please! I promise I won’t text my ex... until after!"),
            new Order("", "I’m ready for a *; let’s get this party started!"),
            new Order("", "One * coming right up, please!"),
            new Order("", "Can you mix me a * that'll make my crush notice me?"),
            new Order("", "Surprise me with a *!"),
            new Order("", "A perfect * would make this night even better."),
            new Order("", "I think a * will make my night perfect."),
            new Order("", "How about a *? I need liquid courage for that exam tomorrow!"),
            new Order("", "I’d like a *—it’s cheaper than therapy!"),
            new Order("", "I’m craving a * right now."),
            new Order("", "I think a * would really hit the spot tonight."),
            new Order("", "I need a refreshing * to cool off!"),
            new Order("", "How about a * for me, please?"),
            new Order("", "I’d love a * to start the night."),
            new Order("", "String cocktail = BadgerBooze();\nif (cocktail.equals(\"*\")) return true;\nelse return false;"),
            //new Order("", "Log.i(“Order”, \"*\")"),
            //new Order("", "printf(\"Can I have a \" + \"*\")"),
        }},

        // Orders for drinks with a specific name
        { "Margarita", new List<Order> {
            new Order("Margarita", "Wasting away again in Margaritaville."),
            new Order("Margarita", "I’d like something with tequila and a refreshing citrus flavor."),
            new Order("Margarita", "Make it a chilled beverage that screams summer!"),
            new Order("Margarita", "Can you whip up a fruity twist on a classic?")
        }},

        { "Bloody Mary", new List<Order> {
            new Order("Bloody Mary", "I need a drink for vampires."),
            new Order("Bloody Mary", "Can you make that tomato-based drink?"),
            new Order("Bloody Mary", "Make it a drink that has lots of flavors going on."),
            new Order("Bloody Mary", "Do you sell tomato soup here?")
        }},

        { "Cosmopolitan", new List<Order> {
            new Order("Cosmopolitan", "I’d like something pink and fruity, please."),
            new Order("Cosmopolitan", "Can you prepare a drink that’s perfect for a night out?"),
            new Order("Cosmopolitan", "I want something classy that looks great in a martini glass."),
            new Order("Cosmopolitan", "Can you whip up a concoction with a citrusy twist?")
        }},

        { "Vesper", new List<Order> {
            new Order("Vesper", "I’m in the mood for a sophisticated, gin-based drink."),
            new Order("Vesper", "I’d like a cocktail that’s strong and refreshing."),
            new Order("Vesper", "Can you prepare a drink with a hint of elegance?"),
            new Order("Vesper", "Once you’ve tasted it, that’s all you want to drink!")
        }},

        { "Whiskey Sour", new List<Order> {
            new Order("Whiskey Sour", "Can you make me something with a whiskey kick and a sour punch?"),
            new Order("Whiskey Sour", "I want something that balances strong whiskey flavor with a citrus twist."),
            new Order("Whiskey Sour", "Amen!"),
            new Order("Whiskey Sour", "Lemon juice and sugar.")
        }},

        { "Paradise", new List<Order> {
            new Order("Paradise", "I’m looking for a fruity concoction with a tropical vibe."),
            new Order("Paradise", "I want a cocktail that feels like a getaway in a glass."),
            new Order("Paradise", "Make it a refreshing drink that’s bright and cheerful."),
            new Order("Paradise", "I’d like something with a blend of spirits and fruit flavors.")
        }},

        { "French 75", new List<Order> {
            new Order("French 75", "Diamond fizz, please!"),
            new Order("French 75", "Can you make me a sparkling drink that’s light but packs a punch?"),
            new Order("French 75", "Can you whip up a drink that’s perfect for celebrations?"),
            new Order("French 75", "Ninety-five? One hundred twenty-five? Never mind.")
        }},

        { "White Lady", new List<Order> {
            new Order("White Lady", "I’d like something that’s smooth, citrusy, and zesty."),
            new Order("White Lady", "Can you prepare a classic drink that’s light and refreshing? The one with a bright color."),
            new Order("White Lady", "I want a cocktail that feels lovely and fancy."),
            new Order("White Lady", "I want a drink that goes well with breakfast!")
        }},

        { "Negroni", new List<Order> {
            new Order("Negroni", "Do you know Conte Camillo Negroni?"),
            new Order("Negroni", "I’m looking for a strong drink with herbal notes."),
            new Order("Negroni", "College life is bittersweet, isn’t it?"),
            new Order("Negroni", "I love sweet vermouth!")
        }},

        { "New York Sour", new List<Order> {
            new Order("New York Sour", "Can you prepare a cocktail that’s strong with a hint of lemon?"),
            new Order("New York Sour", "I’d like something with layers of flavor, please."),
            new Order("New York Sour", "I want a classic with a unique twist on top."),
            new Order("New York Sour", "I want that drink with red wine on top.")
        }}

        /*
        // Order for anything
        new Order("Anything", "Anything you recommend!"),
        new Order("Anything", "01000001 01101110 01111001 01110100 01101000 01101001 01101110 01100111"),
        new Order("Anything", "Could you whip me up a cocktail of the day? I love surprises!"),
        */
    };

    /*
     * Randomly generates a customer order.
     * General orders are more likely to be selected (2/3) than drink-specific orders. (1/3)
     */
    public Order generateCustomerOrder() {
        DrinkManager drinksmanager = GameObject.FindWithTag("DrinksManager").GetComponent<DrinkManager>();
        List<string> drinkNames = drinksmanager.GetUnlockedDrinkNames();
        Debug.Log(drinkNames.Count);
        string randomDrink = drinkNames[UnityEngine.Random.Range(0, drinkNames.Count - 1)];

        int randomOrder = UnityEngine.Random.Range(0, 2);
        Order order;
        // Drink-specific order
        if (randomOrder == 0)
        {
            List<Order> orderList = orderDict[randomDrink];
            order = orderList[UnityEngine.Random.Range(0, orderList.Count - 1)];
        }
        else
        {
            List<Order> orderList = orderDict[""];
            order = orderList[UnityEngine.Random.Range(0, orderList.Count - 1)];
            order.setDrinkName(randomDrink);
        }
        return order;
    }
    
}