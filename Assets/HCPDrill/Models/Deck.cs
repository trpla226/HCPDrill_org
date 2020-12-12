using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck
{
    private HashSet<Card> cards = new HashSet<Card>();
    
    public HashSet<Card> Cards {
        get { return cards; }
    }

    public Deck()
    {
        foreach (int i in Enumerable.Range(0, 4))
        {
            foreach(int j in Enumerable.Range(1,13))
            {
                //Debug.Log($"suit:{i}, number:{j}");
                Cards.Add(new Card((Suit)i, j));
            }
        }
    }
}
