using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IntExt;

public class Card
{
    /// <summary>
    /// 最小のハイカードの強さ。つまりJの強さ値
    /// </summary>
    public const int SmallestHighCardRank = 9;

    /// <summary>
    /// カードの番号とHigh Card Pointの対応
    /// </summary>
    public static readonly Dictionary<int, int> HCPTable;

    #region Constructors
    static Card()
    {
        HCPTable = new Dictionary<int, int>
        {
            {1, 4 }, // A
            {13, 3 }, // K
            {12, 2 }, // Q
            {11, 1 } //J
        };
    }

    public Card(Suit suit, int number)
    {
        Suit = suit;
        Number = number;
    }

    #endregion

    #region Properties
    public Suit Suit { get; }

    public int Number { get; }

    /// <summary>
    /// このカードのHigh Card Point。
    /// A:4
    /// K:3
    /// Q:2
    /// J:1
    /// それ以外:0
    /// </summary>
    public int HCP
    {
        get
        {
            return IsHighCard() ?
                HCPTable[Number] :
                0;
        }
    }

    /// <summary>
    /// カードがK,Q,J,Aかどうか返す
    /// </summary>
    /// <returns>KQJAならtrue,それ以外ならfalse</returns>
    public bool IsHighCard()
    {
        return Rank >= SmallestHighCardRank;
    }

    /// <summary>
    /// デッキ内のカード同士で重複の無い番号
    /// どういうルールで番号をつけるかまだ悩んでる
    /// </summary>
    public int Id { 
        get
        {
            return Mod((int)Suit + Number * 4 - 8 , 52);
        } 
    }

    /// <summary>
    /// カードの数字の強さを返す。
    /// 最強のA=12,
    /// 次に強いK=11,
    /// Q = 10
    /// J = 9
    /// 一番弱い 2 = 0
    /// </summary>
    /// <returns></returns>
    public int Rank
    {
        get { return Mod(Number + 11, 13); }
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

    public string ResourceName {  get
        {
            return string.Format("{0}{1:D2}.png", Suit , Number);
        } 
    }
    #endregion

    #region Methods

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

        return CompareByScore(x, y);  
    }

    public static int CompareBySuit(Card x, Card y) => x.Suit - y.Suit;

    public static int CompareByScore(Card x, Card y) => x.Rank - y.Rank;

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

    public override string ToString()
    {
        return string.Format("{0}{1:D2}", Suit, Number);
    }
    #endregion
}
