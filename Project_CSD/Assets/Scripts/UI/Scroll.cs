using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private GameObject charCollection;
    [SerializeField] private GameObject cardCollection;

    public RectTransform ScrollContent;



    public void setRectPosition()
    {
        float x = ScrollContent.anchoredPosition.x;
        ScrollContent.anchoredPosition = new Vector3(x, 0, 0);
    }
    
    public void CollectionOpen(string type)
    {
        if (type.Equals("spell")){
            charCollection.SetActive(false);
            cardCollection.SetActive(true);
        }
        else if (type.Equals("char"))
        {
            charCollection.SetActive(true);
            cardCollection.SetActive(false);
        }
       
    }

    public void CollectionClear()
    {
        charCollection.SetActive(true);
        cardCollection.SetActive(false);
    }


}
