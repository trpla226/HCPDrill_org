using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using static IntExt;

public class GameController : MonoBehaviour
{
    readonly int randomrange = 10;
    const int dealCount = 13;

    int seed;

    #region MonoBehaviour Overrides
    // Start is called before the first frame update
    void Start()
    {
        seed = DateTime.Now.Millisecond;

        // カードを配る
        var hand = new Hand(Deal());

        foreach(var card in hand.Cards)
        {
            Debug.Log(card.ToString());
        }

        // 選択肢を用意する
        var choices = GenerateChoices(hand.HCP);
        foreach (var choice in choices)
        {
            Debug.Log("choice: " + choice);
        }
        
        var answerButtonController = FindObjectsOfType(typeof(AnswerButtonController))[0] as AnswerButtonController;

        answerButtonController.SetChoices(choices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    
    #region Methods
    /// <summary>
    /// カードを配る
    /// </summary>
    /// <returns></returns>
    List<Card> Deal()
    {
        var deck = new Deck();

        var pickedCardsId = new HashSet<int>();

        var deal = new List<Card>();

        foreach (int _ in Enumerable.Range(0, dealCount)) 
        {
            var card = PickCard(pickedCardsId, deck);
            deal.Add(card);
            pickedCardsId.Add(card.Id);
        }
        deal.Sort(Card.CompareBySuitAndNumber);
        return deal;
    }

    /// <summary>
    /// デッキからランダムにカードを引く
    /// </summary>
    /// <param name="pickedCardsId">すでに引いたカード</param>
    /// <param name="deck">デッキ</param>
    /// <returns>まだ引いてないカード</returns>
    Card PickCard(HashSet<int> pickedCardsId, Deck deck)
    {
        seed++;
        UnityEngine.Random.InitState(seed);
        var id = UnityEngine.Random.Range(1, deck.Cards.Count);
        var card = deck.Cards.Where(c => c.Id == id).Single();

        if (!pickedCardsId.Contains(card.Id))
        {
            return card;
        }
        return PickCard(pickedCardsId, deck);
    }

    /// <summary>
    /// 答えの候補を4種類出す
    /// 正の数であること
    /// </summary>
    /// <param name="answer"></param>
    /// <returns></returns>
    List<int> GenerateChoices(int answer)
    {
        if (!answer.IsValidHCP())
        {
            throw new ArgumentException("answerの値が負か37より大きいです");
        }

        var choices = new List<int>();
        choices.Add(answer);
        while (choices.Count < 4)
        {
            choices.Add(GenerateChoice(answer, choices));
        }
        choices.Sort();
        return choices;
    }

    /// <summary>
    /// ユーザが選択する答えの候補を返す。基準からそれなりに近い値をから選ぶ。
    /// 負の数や37以上や他の選択肢と重複していた場合は、有効な数が出るまで引き直す
    /// </summary>
    /// <param name="answer">正しい答え</param>
    /// <param name="choices">既に挙がっている答えの候補</param>
    /// <returns></returns>
    int GenerateChoice(int answer, List<int> choices)
    {
        seed++;
        UnityEngine.Random.InitState(seed);
        var diff = (int)(UnityEngine.Random.value * randomrange - (randomrange / 2));
        var choice = answer + diff;
        if (!choice.IsValidHCP()
            || choices.Contains(choice))
        {
            return GenerateChoice(answer, choices);
        }
        else
        {
            return choice;
        }
    }
    #endregion
}
