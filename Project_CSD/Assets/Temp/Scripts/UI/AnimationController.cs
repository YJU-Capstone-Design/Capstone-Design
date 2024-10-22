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
        // 모든 SkeletonAnimation과 SkeletonGraphic 찾기
        SkeletonAnimation[] skeletonAnimations = FindObjectsOfType<SkeletonAnimation>();
        SkeletonGraphic[] skeletonGraphics = FindObjectsOfType<SkeletonGraphic>();

        // 모든 SkeletonAnimation 정지
        foreach (var skeletonAnimation in skeletonAnimations)
        {
            skeletonAnimation.AnimationState.ClearTracks();
        }

        // 모든 SkeletonGraphic 정지
        foreach (var skeletonGraphic in skeletonGraphics)
        {
            skeletonGraphic.AnimationState.ClearTracks();
        }

        // 모든 ParticleSystem 정지
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();
        foreach (var particleSystem in particleSystems)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // 모든 Animator 정지
        Animator[] animators = FindObjectsOfType<Animator>();
        foreach (var animator in animators)
        {
            animator.enabled = false; // Animator를 비활성화하여 애니메이션 정지
        }
        Time.timeScale = 0;
    }
}
