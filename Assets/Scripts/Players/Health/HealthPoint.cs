using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;
    private int size;

    public void SetLives(int lives)
    {
        size = lives;
        for (int i = 0; i < lives; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }

        for (int i = lives; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(false);
        }
    }

    public void ExplodeHeart()
    {
        if (size == 0) return;
        heartImages[--size].GetComponent<Animator>().SetTrigger("Destroy");
        StartCoroutine(disable());

    }

    public void AddHeart()
    {
        if(size == 5) return;
        heartImages[size].gameObject.SetActive(true);
        heartImages[size++].GetComponent<Animator>().SetTrigger("Create");
        StartCoroutine(enable());
    }

    private IEnumerator disable()
    {
        yield return new WaitForSeconds(1f);
        heartImages[size].gameObject.SetActive(false);
    }

    private IEnumerator enable()
    {
        yield return new WaitForSeconds(1f);
        heartImages[size - 1].gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}