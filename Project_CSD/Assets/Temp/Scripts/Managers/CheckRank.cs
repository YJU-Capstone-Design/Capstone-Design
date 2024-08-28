using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRank : MonoBehaviour
{
    [SerializeField] GameObject RankList;
    // Update is called once per frame
    public void OpenRankList()
    { RankList.SetActive(true); }

    public void CloseRankList()
    { RankList.SetActive(false); }
}
