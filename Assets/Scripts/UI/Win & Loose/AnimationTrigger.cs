using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private GameEnding gameEnding;

    public void Trigger()
    {
        gameEnding.DeathAnimation();
    }
}
