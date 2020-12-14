﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameController : MonoBehaviour
{
    #region Fields
    // SE
    public AudioClip CorrectAnswerAudioClip;
    public AudioClip WrongAnswerAudioClip;
    public AudioClip TimeUpAudioClip;
    public AudioClip ResultAudioClip;

    public int InitialTimeLimitSeconds = 30;
    public float wrongAnswerPenaltySeconds = 1f;

    // 手札枚数
    const int dealCount = 13;

    // 乱数シード
    int seed;

    // 現在の手札
    private Hand Hand;
    int answer;
    private int score;
    private float currentTimeLimitSeconds;
    bool playerHasAnswered = false;

    // 表示制御
    float opacity; // 正解の〇の不透明度

    // SE再生
    private AudioSource audioSource;


    // ゲームのモジュール
    private AnswerButtonController answerButtonController;
    private HandController handController;
    private ScoreAreaController scoreAreaController;
    private GameObject correctOverlay;
    private GameObject pointsDisplay;

    private TimeLimitAreaController timeLimitAreaController;
    #endregion


    #region MonoBehaviour Overrides
    // Start is called before the first frame update
    IEnumerator Start()
    {
        seed = DateTime.Now.Millisecond;

        currentTimeLimitSeconds = InitialTimeLimitSeconds;
        score = 0;

        // 音声のロード
        audioSource = GetComponent<AudioSource>();


        // 参照キャッシュ
        answerButtonController = FindObjectOfType<AnswerButtonController>();
        handController = FindObjectOfType<HandController>();
        correctOverlay = GameObject.Find("Correct");
        pointsDisplay = GameObject.Find("Points");
        timeLimitAreaController = GameObject.Find("TimeLimitArea").GetComponent<TimeLimitAreaController>();
        scoreAreaController = GameObject.Find("ScoreArea").GetComponent<ScoreAreaController>();
        


        // 中断されるまで繰り返す
        while (currentTimeLimitSeconds > 0) {

            // 手札をクリア
            handController.ClearCards();

            // カードを配る
            var hand = new Hand(Deal());

            // カード表示
            handController.PlaceCards(hand.Cards);

            // 正解表示クリア
            correctOverlay.GetComponent<Image>().SetOpacity(0);


            // 選択肢を用意する
            var choices = GenerateChoices(hand.HCP);
            // 選択肢表示
            answerButtonController.SetChoices(choices);

            bool correctAnswer;
            do
            {
                // プレイヤーが答えるまで待つ
                playerHasAnswered = false;

                // 時間切れの際、WaitPlayerToAnswerは中断される
                yield return StartCoroutine(WaitPlayerToAnswer());
                // 中断時はdo-whileを抜ける
                if (!playerHasAnswered) break;

                correctAnswer = Hand.HCP == answer;

                if (correctAnswer)
                {
                    playerHasAnswered = true;

                    var pointsGain = hand.HighCardCount;
                    score += pointsGain;
                    scoreAreaController.UpdateScore(score);

                    // ボタンの非表示
                    answerButtonController.gameObject.SetActive(false);

                    // 〇を表示
                    correctOverlay.GetComponent<Image>().SetOpacity(1);
                    // 正解の〇をフェードアウトさせる
                    StartCoroutine(UpdateCorrectOverlay());

                    scoreAreaController.DisplayGain(pointsGain);

                    audioSource.PlayOneShot(CorrectAnswerAudioClip);

                    Debug.Log("正解！");
                }
                else
                {
                    // 失敗時ペナルティ
                    currentTimeLimitSeconds -= wrongAnswerPenaltySeconds;
                    audioSource.PlayOneShot(WrongAnswerAudioClip);
                    timeLimitAreaController.DisplayPenalty(wrongAnswerPenaltySeconds);

                    
                    Debug.Log("不正解");
                }
            } while (!correctAnswer);
        }

        // 時間切れになったらゲームオーバー
        audioSource.Stop();
        audioSource.PlayOneShot(TimeUpAudioClip);
        answerButtonController.Disable();

        yield return new WaitForSeconds(3.0f);

        audioSource.clip = ResultAudioClip;
        audioSource.Play();
    }

    private void Update()
    {
        // カウントダウン＆残り時間更新
        currentTimeLimitSeconds -= Time.deltaTime;
        timeLimitAreaController.UpdateTimeLimit(currentTimeLimitSeconds);
    }

    private IEnumerator WaitPlayerToAnswer()
    {
        while (!playerHasAnswered && currentTimeLimitSeconds > 0)
        {
            yield return 0;
        }
        Debug.Log("プレイヤーが回答しました");
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
        deal.Reverse();

        Hand = new Hand(deal);
        answerButtonController.gameObject.SetActive(true);
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
        var choice = UnityEngine.Random.Range(0, 37);
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

    /// <summary>
    /// ユーザが（ボタンをクリックするなどして）解答
    /// </summary>
    /// <param name="answer"></param>
    public void OnAnswer(int answer)
    {
        this.answer = answer;
        playerHasAnswered = true;
    }

    /// <summary>
    /// 正解の〇表示をフェードアウトさせる（Update1回分の処理）
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateCorrectOverlay()
    {
        opacity = 1f;
        var image = correctOverlay.GetComponent<Image>();

        while (opacity >= 0)
        {
            opacity += -0.01f;
            image.SetOpacity(opacity);

            yield return 0;
        }
    }

    #endregion
}
