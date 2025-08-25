using System;
using System.Collections;
using UnityEngine;

public class WinSequence : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform meleePosition;
    [SerializeField] private Transform rangePosition;
    [SerializeField] private GameObject melee;
    [SerializeField] private GameObject range;
    [SerializeField] private Transform meleeAttackZone;
    [SerializeField] private Transform rangeAttackZone;
    [SerializeField] private Vector2 attackBoxSize;
    [SerializeField] private LayerMask hittableLayers;

    [SerializeField] private RectTransform Win;

    public void StartSequence()
    {
        StartCoroutine(PlaySequence());
    }
    
    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.5f);
        melee.GetComponent<Animator>().SetTrigger("Super");
        range.GetComponent<Animator>().SetTrigger("Select");

        yield return new WaitForSeconds(0.5f);
    
        PerformAttackCheck(meleeAttackZone);
        PerformAttackCheck(rangeAttackZone);
    
        yield return new WaitForSeconds(1f);

        Coroutine meleeMove = StartCoroutine(MoveCharacter(melee, meleePosition.position));
        Coroutine rangeMove = StartCoroutine(MoveCharacter(range, rangePosition.position));
    
        yield return meleeMove;
        yield return rangeMove;
    
        UIAnimationManager.Instance.ShowWindow(Win);
    }

    private void PerformAttackCheck(Transform attackZone)
    {
        if (attackZone == null) return;
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackZone.position, attackBoxSize, 0f, hittableLayers);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<Animator>(out Animator animator))
            {
                animator.SetTrigger("Death");
            }
        }
    }

    private IEnumerator MoveCharacter(GameObject character, Vector3 targetPosition)
    {
        character.GetComponent<Animator>().SetBool("Run", true);
        Vector3 characterScale = character.transform.localScale;
        character.transform.localScale = new Vector3(-characterScale.x, characterScale.y, characterScale.z);

        while (Vector3.Distance(character.transform.position, targetPosition) > 0.01f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position,
                targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        character.GetComponent<Animator>().SetBool("Run", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (meleeAttackZone != null)
        {
            Gizmos.DrawWireCube(meleeAttackZone.position, attackBoxSize);
        }
        if (rangeAttackZone != null)
        {
            Gizmos.DrawWireCube(rangeAttackZone.position, attackBoxSize);
        }
    }
}
