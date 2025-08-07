using UnityEngine;

public class CloseRangeEnemy : BaseTopDownEnemies
{
    [SerializeField] private GameObject hitboxUp;
    [SerializeField] private GameObject hitboxDown;
    [SerializeField] private GameObject hitboxLeft;
    [SerializeField] private GameObject hitboxRight;

    [SerializeField] private int knockBackForce;
    
    public void EnableUpHitbox() { hitboxUp.SetActive(true); }
    public void EnableDownHitbox() { hitboxDown.SetActive(true); }
    public void EnableLeftHitbox() { hitboxLeft.SetActive(true); }
    public void EnableRightHitbox() { hitboxRight.SetActive(true); }

    public void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }
    
    public void ApplyDamageAndKnockback(Collider2D player)
    {
        Damagable damagableComponent = player.GetComponent<Damagable>();
        if (damagableComponent != null)
        {
            damagableComponent.Damage(damageAmount);
        }

        BaseControll b = player.GetComponent<BaseControll>();
        if (b != null)
        {
            b.startKnock(knockBackForce);
        }
    
        DisableAllHitboxes();
    }
}
