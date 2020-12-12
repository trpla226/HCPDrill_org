using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    #region Properties

    public Suit Suit { get; }

    public int Number { get; }

    public int Id { 
        get
        {
            return Mod((int)Suit + Number * 4 - 8 , 52);
        } 
    }

    /// <summary>
    /// 答えが負の数にならない剰余
    /// </summary>
    /// <param name="x">割られる数</param>
    /// <param name="y">割る数</param>
    /// <returns>あまり</returns>
    private int Mod(int x, int y)
    {
        return (x + y) % y;
    }

    public string FileName {  get
        {
            return string.Format("{0}{1:D2}.png", Suit , Number);
        } 
    }
    #endregion

    /// <summary>
    /// スートと番号でカードを比較する
    /// スートは大きい方からスペード、ハート、ダイアモンド、クラブの順
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int CompareBySuitAndNumber(Card x, Card y)
    {
        var suitDiff = x.Suit - y.Suit;

        if (suitDiff != 0) return suitDiff;

        return CompareNumber(x, y);  
    }

    public static int CompareSuit(Card x, Card y) => x.Suit - y.Suit;

    public static int CompareNumber(Card x, Card y) => x.Number - y.Number;

    /// <summary>
    /// カードIDで比較する。
    /// IDの仕様はまだ未確定……
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static int CompareById(Card x, Card y)
    {
        if( x == null)
        {
            if(y == null)
            {
                return 0;
            }
            else { 
                return -1;  
            }
        }
        else
        {
            if ( y== null)
            {
                return 1;
            }
            else
            {
                return x.Id - y.Id;
            }
        }
    }

    public Card(Suit suit, int number)
    {
        Suit = suit;
        Number = number;
    }

    public override string ToString()
    {
        return FileName;
        //return Suit + " " + Number;
    }
}
