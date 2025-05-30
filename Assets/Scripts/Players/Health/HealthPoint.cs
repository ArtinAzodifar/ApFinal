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
        //heartImages[index].GetComponent<Animator>().SetTrigger("Explode");
        heartImages[--size].gameObject.SetActive(false);
        
    }

    public void AddHeart()
    {
        if(size == 5) return;
        //heartImages[index].GetComponent<Animator>().SetTrigger("Add");
        heartImages[size++].gameObject.SetActive(true);
    }
}