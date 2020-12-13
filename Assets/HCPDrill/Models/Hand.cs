using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand
{
    public Hand(List<Card> cards)
    {
        if (cards.Count != 13) throw new ArgumentException();
        Cards = cards;
    }

    public List<Card> Cards
    {
        get;
    }

    /// <summary>
    /// 手札のHCP(High Card Point)を返す
    /// </summary>
    public int HCP
    {
        get
        {
            return Cards.Select(card => card.HCP).Sum();
        }
    }

    /// <summary>
    /// ハイカードの枚数を数える
    /// </summary>
    public int HighCardCount
    {
        get
        {
            return Cards.Where(card => card.IsHighCard()).Count();
        }
    }
}
