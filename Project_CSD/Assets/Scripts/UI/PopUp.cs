using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private GameObject stop;

    [Header("PoPMenu")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject pop;
    private void Awake()
    {
        stop.SetActive(false);
        menu.SetActive(false);
        pop.SetActive(false);
    }
    public void OpenMenu()
    {
        stop.SetActive(true);
        menu.SetActive(true);
    }
    public void MenuBtn(string type)
    {
        
        if (type.Equals("Setting"))
        {

        }
        else if (type.Equals("Title"))
        {
            pop.SetActive(true);
        }

    }

    public void Close(GameObject other)
    {
        other.SetActive(false);
        stop.SetActive(false);
    }
}
