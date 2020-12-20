using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// ゲームスタートからゲーム終了の間までの状態を管理する。
/// ゲームセッションのルールを表現する。
/// </summary>
public class Session
{
    internal Hand Hand { get; set; }
    internal int Score { get; set; }

    internal float CurrentTimeLimitSeconds {
        get {
            return currentTimeLimitSeconds;
        }
    }

    internal float elapsedTimeInTurn;


    internal Rule rule;

    // 手札枚数
    private readonly int dealCount = 13;

    internal float currentTimeLimitSeconds;

    private readonly ScoreAreaController scoreAreaController;
    internal readonly TimeLimitAreaController timeLimitAreaController;

    private int answer;
    // 乱数シード
    int seed;

    public Session(ScoreAreaController scoreAreaController,
        TimeLimitAreaController timeLimitAreaController,
        int initialTimeLimitSeconds,
        float wrongAnswerPenaltySeconds) 
    {
        rule = new Rule(this, wrongAnswerPenaltySeconds);

        this.scoreAreaController = scoreAreaController;
        this.timeLimitAreaController = timeLimitAreaController;
        currentTimeLimitSeconds = initialTimeLimitSeconds;
        

        Score = 0;

        seed = DateTime.Now.Millisecond;


    }

    internal void Update()
    {
        currentTimeLimitSeconds -= Time.deltaTime;
    }

    /// <summary>
    /// カードを配る
    /// </summary>
    /// <returns>手札</returns>
    internal void Deal()
    {
        var deck = new Deck();

        // 既に配ったカード
        var pickedCardsId = new HashSet<int>();

        var deal = new List<Card>();

        foreach (int _ in Enumerable.Range(0, dealCount))
        {
            var card = PickCard(pickedCardsId, deck);
            deal.Add(card);
            pickedCardsId.Add(card.Id);
        }

        deal.Sort(Card.CompareBySuitAndNumber);
        deal.Reverse();

        this.Hand = new Hand(deal);
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
    internal List<int> GenerateChoices()
    {
        var correctAnswer = Hand.HCP;
        var choices = new List<int>();
        choices.Add(correctAnswer);
        while (choices.Count < 4)
        {
            choices.Add(GenerateChoice(choices));
        }
        choices.Sort();
        return choices;
    }

    /// <summary>
    /// ユーザが選択する答えの候補を返す。
    /// 負の数や37以上や他の選択肢と重複していた場合は、有効な数が出るまで引き直す
    /// </summary>
    /// <param name="answer">正しい答え</param>
    /// <param name="choices">既に挙がっている答えの候補</param>
    /// <returns></returns>
    int GenerateChoice(List<int> choices)
    {
        seed++;
        UnityEngine.Random.InitState(seed);
        var choice = UnityEngine.Random.Range(0, 37);
        if (!choice.IsValidHCP()
            || choices.Contains(choice))
        {
            return GenerateChoice(choices);
        }
        else
        {
            return choice;
        }
    }

    internal void Answer(int answer) {
        this.answer = answer;
    }

    internal bool AnswerIsCorrect {
        get {
            return Hand.HCP == answer;
        } 
    }

    internal void GainScore(int gain)
    {
        Score += gain;
        scoreAreaController.UpdateScore(Score);
        scoreAreaController.DisplayGain(gain);
    }

    /// <summary>
    /// 持ち時間から減らす。負の値を渡すことで増やすことも可能。
    /// </summary>
    internal void LoseTimeLimit(float penaltyTimeSecond)
    {
        currentTimeLimitSeconds -= penaltyTimeSecond;
        timeLimitAreaController.DisplayPenalty(penaltyTimeSecond);
    }
}
