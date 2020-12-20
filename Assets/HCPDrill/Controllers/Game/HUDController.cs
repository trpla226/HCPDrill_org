using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        correctOverlay = GameObject.Find("Correct");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject correctOverlay;

    // 表示制御
    float opacity; // 正解の〇の不透明度

    internal void DisplaycorrectOverlay()
    {
        // この辺HUD関連オブジェクトにまとめる
        // 〇を表示
        correctOverlay.GetComponent<Image>().SetOpacity(1);
        // 正解の〇をフェードアウトさせる
        StartCoroutine(UpdateCorrectOverlay());
    }

    /// <summary>
    /// 正解の〇表示をフェードアウトさせる（Update1回分の処理）
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateCorrectOverlay()
    {
        opacity = 1f;
        var image = correctOverlay.GetComponent<Image>();

        while (opacity >= 0)
        {
            opacity += -0.01f;
            image.SetOpacity(opacity);

            yield return 0;
        }
    }

}
