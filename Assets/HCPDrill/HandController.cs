using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    /// <summary>
    /// スプライトのリソースが置いてあるフォルダのパス
    /// </summary>
    public readonly string SpritesPath = "PlayingCards/Sprites/";

    public List<GameObject> cards;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceCards(List<Card> cards)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/CardSprite");

        var posX = -6.0f;
        var posY = 0.59f;
        var posZ = 0f;
        var offsetX = 1.2f;
        var offsetZ = -1.0f;

        foreach (var card in cards)
        {
            var obj = Instantiate(prefab);
            obj.transform.position = new Vector3(posX, posY, posZ);

            obj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpritesPath + card.ToString());

            posX += offsetX; // 次のカードの位置
            posZ += offsetZ;
        }
    }
}
