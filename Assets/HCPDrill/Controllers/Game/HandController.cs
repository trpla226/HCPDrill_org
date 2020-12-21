﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandController : MonoBehaviour
{
    /// <summary>
    /// スプライトのリソースが置いてあるフォルダのパス
    /// </summary>
    public readonly string SpritesPath = "PlayingCards/Sprites/";
    public float initialPosX = -8.0f;

    public List<GameObject> Cards;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // TODO: 配られるたびに毎回インスタンス化しているので、パフォーマンス向上のためにインスタンスを使いまわすようにする。

    /// <summary>
    /// カードを画面上に配置する
    /// </summary>
    /// <param name="hand">カードのリスト</param>
    public void PlaceCards(List<Card> hand)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Card");
        var handObject = GameObject.Find("Hand");

        var posX = initialPosX;
        var posY = 0.59f;
        var posZ = 0f;

        // カード一枚あたりずらす量
        var deltaX = 1.2f;
        var deltaZ = -1.0f;

        foreach (var card in hand)
        {
            var cardObject = Instantiate(prefab, handObject.transform);
            cardObject.transform.position = new Vector3(posX, posY, posZ);

            var sprite = cardObject.transform.Find("CardSprite");
            sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpritesPath + card.ToString());

            var pointStr = card.IsHighCard() ? card.HCP.ToString() : string.Empty;
            var pointText = cardObject.transform.Find("Canvas").transform.Find("PointText");
            pointText.GetComponent<TextMeshProUGUI>().text = pointStr;
            pointText.gameObject.SetActive(false);
            

            posX += deltaX; // 次のカードの位置
            posZ += deltaZ; // 重なり順

            Cards.Add(cardObject);
        }
    }

    public void ClearCards()
    {
        foreach (GameObject card in Cards)
        {
            Destroy(card);
        }
        Cards.Clear();
    }
}
