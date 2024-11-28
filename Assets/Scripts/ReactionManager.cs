using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionManager : MonoBehaviour
{

    /* List of reactions for score range of 75-100 */
    private List<string> A_Reactions = new List<string>
    {
        "Perfect! Perfect! Perfect!",
        "This is phenomemal!",
        "I can’t believe how good this tastes.",
        "This is the best drink I’ve ever had.",
        "I’m in love with this drink.",
        "You’ve mastered the art of mixology!",
        "This is exactly what I needed!",
        "If this drink were a person, I’d take it out on a date!",
        "Wow, this is like a party in my mouth, and everyone's invited!",
        "If this drink gets any better, I might need a designated driver... for my taste buds!",
        "This drink is so good, I’d name my firstborn after it.",
        "I’d pay my tuition in this cocktail if I could.",
        "This is what the gods drink on Friday nights.",
        "This drink just solved all my problems… Fantastic.",
        "I bet your GPA is all A.",
        "This drink is like finding a bug-free code—pure bliss!",
        "This cocktail is like a well-structured algorithm—elegant and satisfying!",
        "Eureka!"
    };

    /* List of reactions for score range of 50-74 */
    private List<string> B_Reactions = new List<string>
    {
        "This is pretty decent. I won’t complain.",
        "It’s good enough! Just not one I’d write home about.",
        "I can enjoy it, but it’s not blowing my mind.",
        "Hmm… I’ll give you an AB.",
        "I’m glad I ordered it, but I’d still try something else next time.",
        "It's like debugging a program and finding only one error.",
        "I’ll take this. I wish you can implement a new feature next time!",
        "This drink is like a solid AB, but not quite an A."
    };

    /* List of reactions for score range of 25-49 */
    private List<string> C_Reactions = new List<string>
    {
        "It's... well, it’s a drink.",
        "This is fine, but it’s missing that special something.",
        "Not bad.",
        "I mean, it’s drinkable.",
        "I could take it or leave it. It’s kind of just there.",
        "I was hoping for a little more excitement.",
        "I’ve definitely had better, but it’s okay.",
        "This drink is... functional, I guess? Like the code that works but isn’t really satisfying.",
        "Not great, not terrible. Kind of like getting a C on a project.",
        "Yeah, it’s just alright. Like debugging for hours only to find a simple typo."
    };

    /* List of reactions for score range of 0-24 */
    private List<string> D_Reactions = new List<string>
    {
        "Wow, this is... not what I expected at all. What happened?",
        "This drink is a total letdown. I’m disappointed.",
        "I think I’ll just stick to water.",
        "Do you not know the recipe at all?",
        "This is not it. I’d rather drink a plain soda at this point.",
        "I thought I ordered a cocktail, not flavored water. What’s going on?",
        "Did I order a cocktail or a glass of regret?",
        "Wow, this drink is like a party with no music. Just awkward silence!",
        "This tastes like it went on a vacation and forgot to come back with any flavor.",
        "If this drink were a movie, it would definitely go straight to DVD!",
        "I’d just go drink my assignments.",
        "This drink needs a GPS. It clearly lost its way to deliciousness!",
        "This drink is like a poorly written algorithm—totally inefficient and confusing!",
        "I expected a cocktail, but I got a runtime error instead.",
        "If flavor were an API, this drink would return a 404—Not Found!",
        "This cocktail is like my last project… Looks good on the surface but fails to deliver.",
        "This is like trying to run a program without enough RAM—just not happening.",
        "This drink absolutely needs debugging!"
    };

    /*
     * GetReaction: Returns a random reaction based on the customer's satisfaction score.
     * Parameter:
     *   - score: The satisfaction score of the drink, between 0 and 100.
     */
    public string GetReaction(int score) {
        int random = UnityEngine.Random.Range(0, A_Reactions.Count);
        switch (score)
        {
            case int s when (s >= 75):
                return GetRandomReaction(A_Reactions);
            case int s when (s >= 50):
                return GetRandomReaction(B_Reactions);
            case int s when (s >= 25):
                return GetRandomReaction(C_Reactions);
            default:
                return GetRandomReaction(D_Reactions);
        }
    }

    /*
     * GetRandomReaction: Returns a random reaction from the provided list of reactions.
     * Parameter:
     *   - reactions: The list of reactions to choose from.
     */
    private string GetRandomReaction(List<string> reactions) {
        int random = UnityEngine.Random.Range(0, reactions.Count);
        return reactions[random];
    }
}
