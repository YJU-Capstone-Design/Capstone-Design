using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInit : MonoBehaviour
{
    Button button;

    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void Card_Image(int itemID) // Card Image Setting Method
    {
        switch (itemID)
        {
            // itemID = 11xxx (unit, dealer)
            case 11001:
                // button.targetGraphic = 
                break;
            case 11002:
                break;
            case 11003:
                break;
            case 11004:
                break;
            // itemID = 12xxx (unit, tanker)
            // itemID = 13xxx (unit, supporter)
            // itemID = 21xxx (Spell, Attack)
            case 21001:
                break;
            // itemID = 22xxx (Spell, Buff)
            case 22001:
                break;
            case 22002:
                break;
            case 22003:
                break;
            // itemID = 23xxx (Spell, Debuff)
            case 23001:
                break;

        }
    }
}
