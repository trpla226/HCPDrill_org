using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public readonly string SpritesPath = "PlayingCards/Sprites/";
    public float fadeInSpeed = 0.1f;

    private readonly float Opaque = 1;
    private readonly float Translucent = 0;

    TextMeshProUGUI pointTMP;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initialize(Card card)
    {
        pointTMP = transform.Find("Canvas").transform.Find("PointText")
            .GetComponent<TextMeshProUGUI>();
        var pointStr = card.IsHighCard() ? card.HCP.ToString() : string.Empty;

        var sprite = transform.Find("CardSprite");
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SpritesPath + card.ToString());

        pointTMP.text = pointStr;
        pointTMP.SetOpacity(Translucent);
       
    }

    public void setPointDisplayActive(bool active)
    {
        pointTMP.SetOpacity(active ? Opaque : Translucent);
    }
}
