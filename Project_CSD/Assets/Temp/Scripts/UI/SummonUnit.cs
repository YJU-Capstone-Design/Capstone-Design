using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit : MonoBehaviour
{
    public static SummonUnit instance;
    [Header("FollowerMouse")]
    [SerializeField] SkeletonAnimation summonUnit;
    [SerializeField] GameObject summonUnit_Box;
    void Start()
    {
        instance = this;
    }

    public void GetSkeletonData(Unit data)
    {
        if (data == null)
        {
            summonUnit.skeletonDataAsset = null;
            summonUnit.Initialize(true);
        }
        else
        {
            summonUnit.skeletonDataAsset = data.unit_anim;
            summonUnit.Initialize(true);

            if (data.unitID == 11005) { SummonAnimation("idle_3unit", true); }
            else
            {
                SummonAnimation("Idle", true);
            }

            Debug.Log(summonUnit.skeletonDataAsset);
        }
        
    }

    public void ClearCursor(bool open)
    {
        summonUnit.gameObject.SetActive(open); 

      
    }




    public void SummonAnimation(string animationName, bool loop = false)
    {
        if (summonUnit != null)
        {
            // 애니메이션을 설정하고 상태 변경
            summonUnit.AnimationState.SetAnimation(0, animationName, loop);
        }
        else
        {
            Debug.LogError("SummonUnit이 할당되지 않았습니다!");
        }
    }
}
