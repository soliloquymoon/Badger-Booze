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
        this.customerOrder.Replace("*", drinkName);
    }
}


public class OrderManager {
    /*
     * Static list of all possible orders in the game
     */
    private List<Order> orderList = new List<Order>
    {
        // Random order (replace * with drink name)
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
        new Order("", "Log.i(“Order”, \"*\")"),
        new Order("", "printf(\"Can I have a \" + \"*\""),

        // Orders for drinks with a specific name
        new Order("Margarita", "Wasting away again in Margaritaville."),
        new Order("Margarita", "I’d like something with tequila and a refreshing citrus flavor."),
        new Order("Margarita", "Make it a chilled beverage that screams summer!"),
        new Order("Margarita", "Can you whip up a fruity twist on a classic?"),

        new Order("Bloody Mary", "I need a drink for vampires."),
        new Order("Bloody Mary", "Can you make that tomato-based drink?"),
        new Order("Bloody Mary", "Make it a drink that has lots of flavors going on."),
        new Order("Bloody Mary", "Do you sell tomato soup here?"),

        new Order("Cosmopolitan", "I’d like something pink and fruity, please."),
        new Order("Cosmopolitan", "Can you prepare a drink that’s perfect for a night out?"),
        new Order("Cosmopolitan", "I want something classy that looks great in a martini glass."),
        new Order("Cosmopolitan", "Can you whip up a concoction with a citrusy twist?"),

        new Order("Vesper", "I’m in the mood for a sophisticated, gin-based drink."),
        new Order("Vesper", "I’d like a cocktail that’s strong and refreshing."),
        new Order("Vesper", "Can you prepare a drink with a hint of elegance?"),
        new Order("Vesper", "Once you’ve tasted it, that’s all you want to drink!"),

        new Order("Whiskey Sour", "Can you make me something with a whiskey kick and a sour punch?"),
        new Order("Whiskey Sour", "I want something that balances strong whiskey flavor with a citrus twist."),
        new Order("Whiskey Sour", "Amen!"),
        new Order("Whiskey Sour", "Lemon juice and sugar."),

        new Order("Paradise", "I’m looking for a fruity concoction with a tropical vibe."),
        new Order("Paradise", "I want a cocktail that feels like a getaway in a glass."),
        new Order("Paradise", "Make it a refreshing drink that’s bright and cheerful."),
        new Order("Paradise", "I’d like something with a blend of spirits and fruit flavors."),

        new Order("French 75", "Diamond fizz, please!"),
        new Order("French 75", "Can you make me a sparkling drink that’s light but packs a punch?"),
        new Order("French 75", "Can you whip up a drink that’s perfect for celebrations?"),
        new Order("French 75", "Ninety-five? One hundred twenty-five? Never mind."),

        new Order("White Lady", "I’d like something that’s smooth, citrusy, and zesty."),
        new Order("White Lady", "Can you prepare a classic drink that’s light and refreshing? The one with a bright color."),
        new Order("White Lady", "I want a cocktail that feels lovely and fancy."),
        new Order("White Lady", "I want a drink that goes well with breakfast!"),

        new Order("Negroni", "Do you know Conte Camillo Negroni?"),
        new Order("Negroni", "I’m looking for a strong drink with herbal notes."),
        new Order("Negroni", "College life is bittersweet, isn’t it?"),
        new Order("Negroni", "I love sweet vermouth!"),

        new Order("New York Sour", "Can you prepare a cocktail that’s strong with a hint of lemon?"),
        new Order("New York Sour", "I’d like something with layers of flavor, please."),
        new Order("New York Sour", "I want a classic with a unique twist on top."),
        new Order("New York Sour", "I want that drink with red wine on top.")

        /*
        // Order for anything
        new Order("Anything", "Anything you recommend!"),
        new Order("Anything", "01000001 01101110 01111001 01110100 01101000 01101001 01101110 01100111"),
        new Order("Anything", "Could you whip me up a cocktail of the day? I love surprises!"),

        // Order for high/low ABV
        new Order("High", "Anything strong and bold?"),
        new Order("High", "I bombed my exam."),
        new Order("High", "I’d like a cocktail with a high-proof spirit, please."),
        new Order("High", "Make it a drink that packs a punch and has a strong kick."),
        new Order("High", "Can you whip up something that’s on the stronger side?"),
        new Order("High", "I want a beverage that really delivers on the alcohol content."),
        new Order("High", "I dropped a class! Can you whip me up something strong to celebrate?"),
        new Order("High", "I nailed that test! I’d like a bold cocktail to kick back with."),
        new Order("High", "I got asked out on a date! I need a powerful drink!"),
        new Order("High", "I just went through a breakup. Help me forget about it."),
        new Order("High", "Make it so strong that I forget my problems... and my name!"),
        new Order("Low", "Can you make me something light and easy to sip?"),
        new Order("Low", "I’d prefer a low-alcohol drink to take the edge off."),
        new Order("Low", "Can you whip up something refreshing and mild? I need a break."),
        new Order("Low", "I need to relax."),
        new Order("Low", "I have an exam tomorrow morning."),
        new Order("Low", "I’d like a cocktail with a lower alcohol content, please."),
        new Order("Low","I want something that feels more like a spritzer than a cocktail."),
        new Order("Low", "Can you make me a drink that won’t knock me out? Just a hint of alcohol would be good."),
        new Order("Low", "Just a gentle drink, please."),
        new Order("Low", "I can’t handle my drinks, so can you make it really light for me?")
        */
    };

    /*
     * Randomly generates a customer order.
     */
    public Order generateCustomerOrder() {
        int random = UnityEngine.Random.Range(0, orderList.Count);
        Order order = orderList[random];

        // If the name of the drink is not assigned, randomly assign a drink
        string[] drinkNames = new string[] {
            "Margarita", "Bloody Mary", "Cosmopolitan", "Vesper", "Whiskey Sour",
            "Paradise", "French 75", "White Lady", "Negroni", "New York Sour"
        };
        if (order.getDrinkName() == "") {
            string randomDrink = drinkNames[UnityEngine.Random.Range(0, drinkNames.Length)];
            order.setDrinkName(randomDrink);
        }

        return order;
    }
}