using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine_Ui_Manager : MonoBehaviour
{
    SkeletonGraphic skeletonGraphic;
    void Start()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        if (skeletonGraphic != null)
        {
            Debug.Log("SkeletonGraphic이 성공적으로 초기화되었습니다.");
        }
        else
        {
            Debug.LogError("SkeletonGraphic을 찾을 수 없습니다.");
        }
        skeletonGraphic.AnimationState.SetAnimation(0, "animation", true);
    }
}
