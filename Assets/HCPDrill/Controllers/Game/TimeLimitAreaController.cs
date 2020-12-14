using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeLimitAreaController : MonoBehaviour
{
    private GameObject timeLimitArea;
    private GameObject penaltyLabel;
    private GameObject penaltySpawn;
    private ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        timeLimitArea = GameObject.Find("TimeLimitArea");
        penaltyLabel = GameObject.Find("Penalty");
        penaltySpawn = transform.Find("PenaltySpawn").gameObject;
        progressBar = transform.Find("TimeLimitBar").GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DisplayPenalty(float wrongAnswerPenaltySeconds)
    {
        var newPenaltyLabel = Instantiate(penaltyLabel, penaltySpawn.transform);

        var tmp = newPenaltyLabel.GetComponent<TextMeshProUGUI>();
        tmp.alpha = 1f;
        tmp.text = "-" + wrongAnswerPenaltySeconds;
        Destroy(newPenaltyLabel, 1f);
    }

    internal void UpdateTimeLimit(float currentTimeLimitSeconds)
    {
        progressBar.currentPercent = Math.Max(0, currentTimeLimitSeconds);
    }
}
