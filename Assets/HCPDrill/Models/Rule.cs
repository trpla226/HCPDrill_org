using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    private readonly float wrongAnswerPenaltySeconds;

    internal Func<bool> ShouldProceedToNextTurn;
    internal Action OnWrongAnswer;

    private Session session;

    public Rule(Session session, float wrongAnswerPenaltySeconds)
    {
        this.session = session;

        ShouldProceedToNextTurn = Always;

        OnWrongAnswer = () =>
        {
            session.currentTimeLimitSeconds -= wrongAnswerPenaltySeconds;
            session.timeLimitAreaController.DisplayPenalty(wrongAnswerPenaltySeconds);
        };

    }

    private bool AnswerIsCorrect() => session.AnswerIsCorrect;

    private bool Always() => true;
}
