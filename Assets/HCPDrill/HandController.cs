using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    /// <summary>
    /// スプライトのリソースが置いてあるフォルダのパス
    /// </summary>
    public readonly string SpritesPath = "PlayingCards/Sprites/";

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
        var prefab = Resources.Load<GameObject>("Prefabs/CardSprite");

        var posX = -6.0f;
        var posY = 0.59f;
        var posZ = 0f;
        var offsetX = 1.2f;
        var offsetZ = -1.0f;

        foreach (var card in hand)
        {
            var obj = Instantiate(prefab);
            obj.transform.position = new Vector3(posX, posY, posZ);

            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpritesPath + card.ToString());

            posX += offsetX; // 次のカードの位置
            posZ += offsetZ;

            Cards.Add(obj);
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
