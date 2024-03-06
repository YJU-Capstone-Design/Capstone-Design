using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    public static int a = 0;

    public void Start()
    {
        main x = new main();
        main y = new main();
        x.SetHP(1);
        Debug.Log(a);
        Debug.Log(a);
        y.SetHP(2);
        Debug.Log(a);

        Debug.Log(a);

    }

    public void SetHP
        (int i)
    {
        a = i;
    }
    // Update is called once per frame
    /*public int GetHP()
    {
        return a;
    }*/
}
