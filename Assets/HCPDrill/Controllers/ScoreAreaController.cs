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

    internal void DisplayGain(int gain)
    {
        var newGainLabel = Instantiate(gainLabel, scoreArea.transform);

        var tmp = newGainLabel.GetComponent<TextMeshProUGUI>();
        tmp.alpha = 1f;
        tmp.text = "+" + gain;
        Destroy(newGainLabel, 1f);
    }

    internal void UpdateScore(int score)
    {
        points.GetComponent<TextMeshProUGUI>().text = score.ToString();

    }
}
