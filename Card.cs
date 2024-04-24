using System.Xml.Linq;

// Add more spacing between your variables / properties / attributes / methods. This class is cramped and hard to read.
// I have gone ahead and made some space.
public class Card
{
    // This static variable will affect all instances of your Card class. This should be an attribute initialised in the ctor (or a property).
    public static int handLimit = 5; // Change when desired

    // Static random is good, means all calls to this should be different.
    static Random random = new Random();
    
    // static deck, hand, cardsInDeck means all your Card instances share the same values. Will create bugs. These should be properties.
    public static List<Card> deck = new List<Card>();
    public static List<Card> hand = new List<Card>();
    static int cardsInDeck;

    // The value attribute is never used. Remove it.
    // Calling the number of the card "title" makes little sense in this context. Value was a more fitting variable name.
    public int value; // Parameter 1
    public string suite; // Parameter 2
    public string title; // Parameter 3

    // ctor is being used by the static CreateDeck() method, therefore it probably should be private.
    // Otherwise you could create a deck without using the CreateDeck() method.
    public Card(int value, string suite, string title) // btw ctor is a shorthand for 'C onstruc TOR'
    {
        this.value = value;
        this.suite = suite;
        this.title = title;
    }

    // You only need to do this kind of method to create an object when using a factory pattern. All this code should be
    // in the ctor.
    public static void CreateDeck()
    {
        // Although this is not necessary, these could be static readonly arrays of the class instead of in this method.
        // These are basically "magic strings".
        string[] title = {"two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "ace"};
        string[] royalTitle = {"jack", "queen", "king"};

        // Weird to start your for loops like this, and also makes it harder to understand for other programmers.
        for (int i = 2; i <= 11; i++)
        {
            // Scope for int i = 2 starts
            deck.Add(new Card(i, "clubs", title[i - 2]));
            deck.Add(new Card(i, "spades", title[i - 2]));
            deck.Add(new Card(i, "hearts", title[i - 2]));
            deck.Add(new Card(i, "diamonds", title[i - 2]));
            // Scope for int i = 2 ends
        }
        // int i doesn't exist here anymore.
        
        // You don't need to use 'j' here as a variable since the previously used 'i' variable in your for loop
        // is not in the same scope. You won't have conflicts.
        for (int j = 0; j < 3; j++)
        {
            deck.Add(new Card(10, "clubs", royalTitle[j]));
            deck.Add(new Card(10, "spades", royalTitle[j]));
            deck.Add(new Card(10, "hearts", royalTitle[j]));
            deck.Add(new Card(10, "diamonds", royalTitle[j]));
        }
        cardsInDeck = deck.Count(); // Either make this a computed property / attribute or just don't assign it at all and call deck.Count() every time.
    }

    // Usually you would not return void very often.
    public static void EraseDeck(bool createNewDeck)
    {
        deck.Clear();
        Console.WriteLine("Deck erased!");
        
        if (createNewDeck)
        {
            CreateDeck();
        }
    }

    // I saw your commit where your argument was named 'int howMany' and now you changed it to 'howmany'.
    // In C#, you follow camelCase for attributes, variables and arguments
    // and PascalCase for properties, classes (and interfaces, structs, records, etc) and method names.
    // This argument should be reverted to 'howMany'.
    public static void RemoveCardsFromHand(int howmany) 
    {
        // This method logic is slightly flawed in that you are removing directly from the hand in a specific order.
        // I.e: remove from the top, remove from the bottom, remove random etc.
        for (int i = 0; i < howmany; i++)
        {
            hand.RemoveAt(i);
        }
    }

    // This should probably return a Card object or something
    public static void Draw(int howMany)
    {
        // Sure?
        // Unrelated to your logic, what you did is very close to a guard clause.
        // You should express these (if possible) like so:
        /*
        if (condition)
            return;
        */
        if (handLimit > cardsInDeck)
        {
            handLimit = cardsInDeck;
        }

        for (int i = 1; i <= howMany; i++)
        {
            if (hand.Count() < handLimit)
            {
                Card SelectedCard = deck[random.Next(0, cardsInDeck)];
                hand.Add(SelectedCard);
                deck.Remove(SelectedCard);
                // this code is all fine and good but nowhere are you returning the card nor the hand, so this effectively does nothing except remove cards from the deck
                // similar to calling RemoveCardsFromHand(howMany);
            }
            else
            {
                Console.WriteLine("Hand limit reached.");
            }
            // You should also inverse this logic to aim for the 'happy path', in this context being "drawing cards from the hand".
            // To do this, structure your code like so:
            /*
                if (hand.Count() >= handLimit)-> unhappy path
                {
                    Console.WriteLine("Hand limit reached");
                    return;
                } -> this could also be your guard clause like I alluded to earlier on lines 95-104.
                
                happy path:
                Card SelectedCard = deck[random.Next(0, cardsInDeck)];
                hand.Add(SelectedCard);
                deck.Remove(SelectedCard);
            */
            // what this does is remove a level of indentation in your happy path, so it's easier to read.
        }
    }
}