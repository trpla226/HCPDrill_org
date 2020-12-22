using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.UI.ModernUIPack;
using UnityEngine.SceneManagement;

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
    public float HCPHintSeconds = 8f;


    // 現在の手札
    private Session session;
    bool playerHasAnswered = false;

    // SE再生
    private AudioSource audioSource;


    // ゲームのモジュール
    private HUDController HUDController;
    private AnswerButtonController answerButtonController;
    private HandController handController;
    private ScoreAreaController scoreAreaController;

    private TimeLimitAreaController timeLimitAreaController;


    #endregion


    #region MonoBehaviour Overrides
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // 参照キャッシュ
        HUDController = GameObject.Find("HUD").GetComponent<HUDController>();
        answerButtonController = FindObjectOfType<AnswerButtonController>();
        handController = FindObjectOfType<HandController>();
        timeLimitAreaController = GameObject.Find("TimeLimitArea").GetComponent<TimeLimitAreaController>();
        scoreAreaController = GameObject.Find("ScoreArea").GetComponent<ScoreAreaController>();

        // このオブジェクトのコンポーネント
        audioSource = GetComponent<AudioSource>();

        session = new Session(scoreAreaController, 
            timeLimitAreaController,
            InitialTimeLimitSeconds,
            wrongAnswerPenaltySeconds);

        
        // 中断されるまで繰り返す
        while (session.CurrentTimeLimitSeconds > 0) {
            session.elapsedTimeInTurn = 0f;

            // カードを配る
            session.Deal();

            // 手札をクリア
            handController.ClearCards();

            // カード表示
            handController.PlaceCards(session.Hand.Cards);

            // 選択肢を用意する
            var choices = session.GenerateChoices();
            // 選択肢表示
            answerButtonController.SetChoices(choices);

            do
            {
                // プレイヤーが答えるまで待つ
                playerHasAnswered = false;

                // 時間切れの際、WaitPlayerToAnswerは中断される
                yield return StartCoroutine(WaitPlayerToAnswer());
                // 中断時はdo-whileを抜ける
                if (!playerHasAnswered) break;

                if (session.AnswerIsCorrect)
                {
                    playerHasAnswered = true;

                    session.rule.OnCorrectAnswer();

                    HUDController.DisplaycorrectOverlay();

                    audioSource.PlayOneShot(CorrectAnswerAudioClip);
                }
                else
                {
                    // 失敗時ペナルティ
                    session.rule.OnWrongAnswer();

                    audioSource.PlayOneShot(WrongAnswerAudioClip);
                }
            } while (!session.rule.ShouldProceedToNextTurn());
        }

        // 時間切れになったらゲームオーバー
        audioSource.Stop();
        audioSource.PlayOneShot(TimeUpAudioClip);
        answerButtonController.Disable();

        var modalWindow = GameObject.Find("ResultModal").GetComponent<ModalWindowManager>();
        modalWindow.titleText = "Result";
        modalWindow.descriptionText = $"Score: {session.Score}\nMiss: 0";
        modalWindow.UpdateUI();
        modalWindow.OpenWindow();

        yield return new WaitForSeconds(3.0f);

        audioSource.clip = ResultAudioClip;
        audioSource.Play();
    }

    private void Update()
    {
        // カウントダウン＆残り時間更新
        session.Update();
        timeLimitAreaController.UpdateTimeLimit(session.CurrentTimeLimitSeconds);

        if(session.elapsedTimeInTurn > HCPHintSeconds)
        {
            handController.ShowHCPHint();
        }
    }
    #endregion


    #region Methods
    private IEnumerator WaitPlayerToAnswer()
    {
        while (!playerHasAnswered && session.CurrentTimeLimitSeconds > 0)
        {
            session.elapsedTimeInTurn += Time.deltaTime;
            yield return 0;
        }
        Debug.Log("プレイヤーが回答しました");
    }

    /// <summary>
    /// タイトル画面への遷移
    /// </summary>
    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// ユーザが（ボタンをクリックするなどして）解答
    /// </summary>
    /// <param name="answer"></param>
    public void OnAnswer(int answer)
    {
        session.Answer(answer);
        playerHasAnswered = true;
    }

    #endregion
}
