using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Card card in new Deck().Cards)
        {
            //Debug.Log(card.ToString() + " ID:" + card.Id);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
