using System;
using System.Collections;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private RectTransform text;
    [SerializeField] private RectTransform restart;
    [SerializeField] private RectTransform exit;

    private void Start()
    {
        text.localScale = Vector3.zero;
        if (restart != null ) restart.localScale = Vector3.zero;
        if (exit != null ) exit.localScale = Vector3.zero;
        
        StartCoroutine(AnimationTrigger());
    }

    private IEnumerator AnimationTrigger()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        UIAnimationManager.Instance.ShowWindow(text);

        if (restart != null)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            UIAnimationManager.Instance.ShowWindow(restart);
        }

        if (exit != null)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            UIAnimationManager.Instance.ShowWindow(exit);
        }
    }
}