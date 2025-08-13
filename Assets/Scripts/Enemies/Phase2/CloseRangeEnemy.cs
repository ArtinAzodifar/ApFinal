using UnityEngine;

public class CloseRangeEnemy : BaseTopDownEnemies
{
    [SerializeField] private GameObject hitboxUp;
    [SerializeField] private GameObject hitboxDown;
    [SerializeField] private GameObject hitboxLeft;
    [SerializeField] private GameObject hitboxRight;

    [SerializeField] private int knockBackForce;

    //these methods called in the middle of animations
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
        if (!gameManager.IsLocalMode() && !IsServer) return;

        Damagable damagableComponent = player.GetComponent<Damagable>();
        if (damagableComponent != null)
        {
            damagableComponent.Damage(damageAmount);
        }

        if (gameManager.IsLocalMode())
        {
            BaseControll b = player.gameObject.GetComponent<BaseControll>();
            b.setKnockFromRight(player.gameObject.transform.position.x <= transform.position.x);
            b.startKnock(knockBackForce);
        }
        else if (IsServer)
        {
            BaseControll b = player.gameObject.GetComponent<BaseControll>();
            b.setKnockFromRightClientRpc(player.gameObject.transform.position.x <= transform.position.x);
            b.startKnockClientRpc(knockBackForce);
        }
    
        DisableAllHitboxes();
    }
}
