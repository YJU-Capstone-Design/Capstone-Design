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
            Debug.Log("SkeletonGraphic�� ���������� �ʱ�ȭ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("SkeletonGraphic�� ã�� �� �����ϴ�.");
        }
        skeletonGraphic.AnimationState.SetAnimation(0, "animation", true);
    }
}
