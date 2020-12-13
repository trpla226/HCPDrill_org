﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeLimitAreaController : MonoBehaviour
{
    private GameObject timeLimitArea;
    private GameObject timeLimitDisplay;
    private GameObject penaltyLabel;

    // Start is called before the first frame update
    void Start()
    {
        timeLimitArea = GameObject.Find("TimeLimitArea");
        penaltyLabel = GameObject.Find("Penalty");
        timeLimitDisplay = GameObject.Find("TimeLimit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DisplayPenalty(float wrongAnswerPenaltySeconds)
    {
        var newPenaltyLabel = Instantiate(penaltyLabel, timeLimitArea.transform);
        newPenaltyLabel.GetComponent<TextMeshProUGUI>().alpha = 1f;
        Destroy(newPenaltyLabel, 1f);
    }

    internal void UpdateTimeLimit(float currentTimeLimitSeconds)
    {
        int displaySeconds = (int)Math.Max(0, Math.Ceiling(currentTimeLimitSeconds));
        timeLimitDisplay.GetComponent<TextMeshProUGUI>().SetText(displaySeconds.ToString());
    }
}
