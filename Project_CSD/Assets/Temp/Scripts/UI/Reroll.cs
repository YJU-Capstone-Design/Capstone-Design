using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Reroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] GameObject reroll_Cost;
    [SerializeField] GameObject reroll_Icon;

    public void OnPointerEnter(PointerEventData eventData)
    {
        reroll_Cost.SetActive(true);
    }

    // Implement IPointerExitHandler's OnPointerExit method
    public void OnPointerExit(PointerEventData eventData)
    {
        reroll_Cost.SetActive(false);
    }
}
