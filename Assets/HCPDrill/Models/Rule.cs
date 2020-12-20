using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rule
{
    private readonly float wrongAnswerPenaltySeconds;

    internal Func<bool> ShouldProceedToNextTurn;
    internal Action OnWrongAnswer;


    public Rule(Session session, float wrongAnswerPenaltySeconds)
    {
        ShouldProceedToNextTurn = () => 
        {
            return session.AnswerIsCorrect;
        };

        OnWrongAnswer = () =>
        {
            session.currentTimeLimitSeconds -= wrongAnswerPenaltySeconds;
            session.timeLimitAreaController.DisplayPenalty(wrongAnswerPenaltySeconds);
        };

    }

}
