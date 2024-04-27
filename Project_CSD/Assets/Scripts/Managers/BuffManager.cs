using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    PlayerSpell playerSpell;
    PlayerUnit playerUnit;

    private void Awake()
    {
        playerSpell = GetComponent<PlayerSpell>();
        playerUnit = GetComponent<PlayerUnit>();
    }

    public void UsingCard()
    {
        int value = this.playerSpell.value;

        switch (value)
        {
            case 20000:
                SpellManager.Instance.Buff_Logic(value);
                Debug.Log("ATK_UP");
                break;
            case 20001:
                SpellManager.Instance.Buff_Logic(value);
                Debug.Log("SPEED_UP");
                break;
            case 22001:
                SpellManager.Instance.Buff_Logic(value);
                Debug.Log("HEAL");
                break;
        }
    }
}
