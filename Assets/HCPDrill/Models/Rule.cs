using System;

/// <summary>
/// ゲームのルールを表現する
/// </summary>
public class Rule
{
    private readonly float wrongAnswerPenaltySeconds;

    internal Func<bool> ShouldProceedToNextTurn;
    internal Action OnCorrectAnswer;
    internal Action OnWrongAnswer;

    private readonly Session session;

    /// <summary>
    /// ターン中持ち時間。
    /// この時間内に回答した場合、経過時間 * 係数のタイムボーナス。
    /// </summary>
    private readonly float timeLimitOfATurn = 8f;

    /// <summary>
    /// タイムボーナス係数
    /// </summary>
    private readonly float timeBonusCoefficient = 0.9f;

    public Rule(Session session, float wrongAnswerPenaltySeconds)
    {
        this.session = session;
        this.wrongAnswerPenaltySeconds = wrongAnswerPenaltySeconds;

        ShouldProceedToNextTurn = Always;

        // 正解時の処理
        OnCorrectAnswer = () =>
        {
            var gain = session.Hand.HighCardCount;
            session.GainScore(gain);

            var timeBonus = Math.Min(timeLimitOfATurn, session.elapsedTimeInTurn) 
                            * timeBonusCoefficient;
            session.LoseTimeLimit( - (float)timeBonus );
        };

        // 間違えた場合の処理
        OnWrongAnswer = () =>
        {
            session.LoseTimeLimit(this.wrongAnswerPenaltySeconds);
        };

    }

    /// <summary>
    /// プレイヤーの答えが正しいかどうか
    /// </summary>
    /// <returns>正解:true 不正解:false</returns>
    private bool AnswerIsCorrect() => session.AnswerIsCorrect;


    /// <summary>
    /// 常にtrue
    /// ShouldProceedToNextTurnデリゲートに設定することで必ず次のターンに
    /// 遷移させることができる
    /// </summary>
    /// <returns></returns>
    private bool Always() => true;
}
