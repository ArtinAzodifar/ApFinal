using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class P2SuperShoot : NetworkBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ManaBar manaBar;
    
    public int mana;
    private int maxMana;
    private Animator animator;
    public NetworkVariable<int> mana = new NetworkVariable<int>(0);
    
    [SerializeField] private GameObject laserSegmentPrefab;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private float segmentSpacing = 1f;
    [SerializeField] private int damageAmount;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Start()
    {
        maxMana = 10;
        manaBar.SetMaxMana(maxMana);
        
        if (SaveManager.Instance == null || !SaveManager.Instance.IsGameLoaded)
        {
            mana = 0;
            manaBar.SetMana(mana);
        }
    }

    private void OnEnable()
    {
        ArrowController.P2Mana += ManaAdd;
        FullMana.ManaFill += MaxMana;
    }

    private void OnDisable()
    {
        ArrowController.P2Mana -= ManaAdd;
        FullMana.ManaFill -= MaxMana;
    }

    public void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsOwner) return;
        if (context.performed && !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana.Value >= MAX_MANA)
        {
            mana = 0;
            manaBar.SetMana(mana);
            StartCoroutine(SuperAttack());
        }
    }
    [ServerRpc]
    private void applySuperShootServerRpc()
    {
        mana.Value = 0;
        StartCoroutine(SuperAttack());
        resetManaBarClientRpc();
    }
    [ClientRpc]
    private void resetManaBarClientRpc() { manaBar.SetMaxMana(MAX_MANA); }

    private IEnumerator SuperAttack()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) yield break; //only local mode or the server(in online mode) can start this
        animator.SetTrigger("SuperShoot");
        yield return new WaitForSeconds(0.3f);
        float pastTime = 0;
        float maxTime = 1.9f;
        while (pastTime < maxTime)
        {
            pastTime += Time.deltaTime;
            Vector2 boxCenter = transform.position;
            boxCenter.x += gameObject.transform.localScale.x > 0 ? 4.5f : -4.5f;
            Vector2 boxSize = new Vector2(9f, 2f);
            Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);
            foreach (Collider2D enemy in hitEnemy)
            {
                if (enemy.gameObject.GetComponent<Damagable>() != null)
                {
                    enemy.gameObject.GetComponent<Damagable>().Damage(damageAmount);
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.9f);
    }

    private void ManaAdd()
    {
        if (GameManager.Instance.IsLocalMode()) mana.Value++; // local mode
        else if (IsOwner) addManaServerRpc(); // in online mode only server modifies mana value, and only the owner can ask for it
        manaBar.addMana();
    }
    [ServerRpc]
    private void addManaServerRpc() { mana.Value++; }

    private void MaxMana(GameObject g)
    {
        if (g != gameObject) return;
        if (GameManager.Instance.IsLocalMode()) {mana.Value = MAX_MANA;} // local mode
        else if (IsOwner) maxManaServerRpc(); // in online mode only server modifies mana value, and only the owner can ask for it
        manaBar.FillMana();
    }
    [ServerRpc]
    private void maxManaServerRpc() { mana.Value = MAX_MANA; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = transform.position;
        boxCenter.x += gameObject.transform.localScale.x > 0 ? 4.5f : -4.5f;
        Vector2 boxSize = new Vector2(9f, 2f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    private void ShowLaserAnimation()//is called in the middle of super shoot animation
    {
        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        for (int i = 0; i < laserCount; i++)
        {
            Vector3 spawnPos = laserStartPoint.position + direction * segmentSpacing * i;
            GameObject laserSegment = Instantiate(laserSegmentPrefab, spawnPos, Quaternion.identity);

            //spawn object if it is online mode
            if (!GameManager.Instance.IsLocalMode() && IsServer) laserSegment.GetComponent<NetworkObject>().Spawn();

            if (transform.localScale.x < 0)
            {
                Vector3 scale = laserSegment.transform.localScale;
                scale.x *= -1;
                laserSegment.transform.localScale = scale;
            }
        }
    }

    public int getMana()
    {
        return mana.Value;
    }

    public void setMana(int amount)//this method is only for save/load which is only for local mode
    {
        this.mana = amount;
        manaBar.SetMana(this.mana);
    }
}