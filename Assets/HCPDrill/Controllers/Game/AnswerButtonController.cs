using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using System;
using System.Linq;
using UnityEngine.UI;

public class AnswerButtonController : MonoBehaviour
{
    public List<GameObject> buttons;

    public GameController gameController;

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
    /// <param name="choices">選択肢</param>
    internal void SetChoices(List<int> choices)
    {
        foreach (var i in Enumerable.Range(0, 4))
        {
            var answerValue = choices[i];

            var buttonObject = buttons[i];
            var button = buttonObject.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => gameController.OnAnswer(answerValue));
            button.enabled = true;
            button.interactable = true;

            var buttonManager = buttonObject.GetComponent<ButtonManagerBasic>();
            buttonManager.buttonText = answerValue.ToString();
            buttonManager.enabled = true;
            buttonManager.UpdateUI();
        }
    }

    /// <summary>
    /// ボタンを操作不能にする
    /// 半透明でリップルも出ないようにする。git 
    /// </summary>
    internal void Disable()
    {
        foreach(GameObject b in buttons)
        {
            var button = b.GetComponent<Button>();
            button.enabled = false;
            button.interactable = false;

            var buttonManager = b.GetComponent<ButtonManagerBasic>();
            buttonManager.enabled = false;
        }
    }
}
