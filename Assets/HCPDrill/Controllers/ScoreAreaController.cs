using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreAreaController : MonoBehaviour
{
    private GameObject scoreArea;
    private GameObject points;
    private GameObject gainLabel;

    // Start is called before the first frame update
    void Start()
    {
        scoreArea = GameObject.Find("ScoreArea");
        points = GameObject.Find("Points");
        gainLabel = GameObject.Find("PointsGain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 獲得したポイントを点数表示の近くに表示する
    /// </summary>
    /// <param name="gain">獲得したポイント</param>
    internal void DisplayGain(int gain)
    {
        var newGainLabel = Instantiate(gainLabel, scoreArea.transform);

        var tmp = newGainLabel.GetComponent<TextMeshProUGUI>();
        tmp.alpha = 1f;
        tmp.text = "+" + gain;
        Destroy(newGainLabel, 1f);
    }

    /// <summary>
    /// 点数表示の更新
    /// </summary>
    /// <param name="score">現在の点数</param>
    internal void UpdateScore(int score)
    {
        points.GetComponent<TextMeshProUGUI>().text = score.ToString();

    }
}
