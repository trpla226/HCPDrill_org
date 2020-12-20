using Michsky.UI.ModernUIPack;
using System;
using TMPro;
using UnityEngine;

public class TimeLimitAreaController : MonoBehaviour
{
    private Color gainColor;

    private GameObject penaltyLabel;
    private GameObject penaltySpawn;
    private ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        // ペナルティではなくボーナスの時のテキスト色
        gainColor = "#00B915".ToColor();

        penaltyLabel = GameObject.Find("Penalty");
        penaltySpawn = transform.Find("PenaltySpawn").gameObject;
        progressBar = transform.Find("TimeLimitBar").GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DisplayPenalty(float penaltySeconds)
    {
        var newPenaltyLabel = Instantiate(penaltyLabel, penaltySpawn.transform);
        var tmp = newPenaltyLabel.GetComponent<TextMeshProUGUI>();
        Destroy(newPenaltyLabel, 1f);
        
        tmp.alpha = 1f;

        if (penaltySeconds > 0)
        {

            tmp.text = "-" + penaltySeconds;
        } else
        {
            var displaySec = - penaltySeconds;

            tmp.color = gainColor;
            tmp.alpha = 1f;
            tmp.text = "+" + ((int)Math.Ceiling(displaySec)).ToString();
        }
    }

    internal void UpdateTimeLimit(float currentTimeLimitSeconds)
    {
        progressBar.currentPercent = Math.Max(0, currentTimeLimitSeconds);
    }
}
