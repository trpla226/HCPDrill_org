using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using System;
using System.Linq;

public class AnswerButtonController : MonoBehaviour
{
    public List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 答えの候補を選択肢に反映
    /// </summary>
    /// <param name="choices"></param>
    internal void SetChoices(List<int> choices)
    {
        foreach (var i in Enumerable.Range(0, 4))
        {
            var button = buttons[i].GetComponent<ButtonManagerBasic>();
            button.buttonText = choices[i].ToString();
            button.UpdateUI();
        }


    }
}
