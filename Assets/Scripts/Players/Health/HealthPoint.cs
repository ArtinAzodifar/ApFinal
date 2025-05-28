using UnityEngine;
using UnityEngine.UI;

public class HealthPoint : MonoBehaviour
{
    [SerializeField] private Image[] heartImages;

    public void SetLives(int lives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(true);
        }
    }

    public void ExplodeHeart(int index)
    {
        // Optional: add explosion animation here
        // Example: heartImages[index].GetComponent<Animator>().SetTrigger("Explode");
        heartImages[index].gameObject.SetActive(false);
    }
}