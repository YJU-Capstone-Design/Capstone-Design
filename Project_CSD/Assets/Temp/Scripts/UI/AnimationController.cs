using UnityEngine;
using Spine.Unity;

public class AnimationController : MonoBehaviour
{
    public static AnimationController instance;
    private void Start()
    {
        instance = this;
    }
    public void StopAllAnimations()
    {
        // ��� SkeletonAnimation�� SkeletonGraphic ã��
        SkeletonAnimation[] skeletonAnimations = FindObjectsOfType<SkeletonAnimation>();
        SkeletonGraphic[] skeletonGraphics = FindObjectsOfType<SkeletonGraphic>();

        // ��� SkeletonAnimation ����
        foreach (var skeletonAnimation in skeletonAnimations)
        {
            skeletonAnimation.AnimationState.ClearTracks();
        }

        // ��� SkeletonGraphic ����
        foreach (var skeletonGraphic in skeletonGraphics)
        {
            skeletonGraphic.AnimationState.ClearTracks();
        }

        // ��� ParticleSystem ����
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // ��� Animator ����
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (var animator in animators)
        {
            animator.enabled = false; // Animator�� ��Ȱ��ȭ�Ͽ� �ִϸ��̼� ����
        }
        Time.timeScale = 0;
    }
}
