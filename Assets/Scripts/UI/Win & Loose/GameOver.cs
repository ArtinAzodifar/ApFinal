using System.Collections;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    [SerializeField] private RectTransform gameOver;
    [SerializeField] private RectTransform restart;
    [SerializeField] private RectTransform exit;
    
    [SerializeField] private Animator skeletonAnimator;
    [SerializeField] private Animator meleeAnimator;
    [SerializeField] private Animator rangeAnimator;

    private IEnumerator AnimationTrigger()
    {
        UIAnimationManager.Instance.ShowWindow(gameOver);
        yield return new WaitForSecondsRealtime(0.5f);
        
        skeletonAnimator.SetTrigger("Attack");
        
        UIAnimationManager.Instance.ShowWindow(restart);
        yield return new WaitForSecondsRealtime(0.2f);
        UIAnimationManager.Instance.ShowWindow(exit);
    }

    public void DeathAnimation()
    {
        meleeAnimator.SetTrigger("Death");
        rangeAnimator.SetTrigger("Death");
    }
}
