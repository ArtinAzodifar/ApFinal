using Unity.Netcode;
using UnityEngine;

public class LongRangeEnemy : BaseTopDownEnemies
{
    [SerializeField] private GameObject attackPoint_Up;
    [SerializeField] private GameObject attackPoint_Down;
    [SerializeField] private GameObject attackPoint_Left;
    [SerializeField] private GameObject attackPoint_Right;
    [SerializeField] private GameObject projectilePrefab;

    private Transform _currentAttackPoint;
    
    public void SetAttackPointUp() { _currentAttackPoint = attackPoint_Up.transform; }
    public void SetAttackPointDown() { _currentAttackPoint = attackPoint_Down.transform; }
    public void SetAttackPointLeft() { _currentAttackPoint = attackPoint_Left.transform; }
    public void SetAttackPointRight() { _currentAttackPoint = attackPoint_Right.transform; }

    public void FireProjectile()
    {
        if (!gameManager.IsLocalMode() && !IsServer) return;

        if (projectilePrefab != null || _currentAttackPoint != null)
        {
            GameObject projectileObj = Instantiate(projectilePrefab,
                _currentAttackPoint.position, _currentAttackPoint.rotation);

            var netObj = projectileObj.GetComponent<NetworkObject>();
            if (!gameManager.IsLocalMode() && netObj != null) netObj.Spawn();
        }
    }
}