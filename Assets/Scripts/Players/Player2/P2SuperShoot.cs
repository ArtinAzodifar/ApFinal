using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class P2SuperShoot : NetworkBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private ManaBar manaBar;

    private const int MAX_MANA = 10;
    private Animator animator;
    public NetworkVariable<int> mana = new NetworkVariable<int>(0);

    [SerializeField] private GameObject laserSegmentPrefab;
    [SerializeField] private Transform laserStartPoint;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private float segmentSpacing = 1f;
    [SerializeField] private int damageAmount;
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        manaBar.SetMaxMana(MAX_MANA);

        if (gameManager.IsLocalMode() && (SaveManager.Instance == null || !SaveManager.Instance.IsGameLoaded))
        {
            mana.Value = 0;
            manaBar.SetMana(mana.Value);
        }
        else if (!gameManager.IsLocalMode() && IsServer)
        {
            mana.Value = 0;
            UpdateManaBarClientRpc(mana.Value);
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
        if (context.performed && !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana.Value >= MAX_MANA && !gameObject.GetComponent<PlayerHealth>().IsDying())
        {
            if (GameManager.Instance.IsLocalMode())
            {
                mana.Value = 0;
                manaBar.SetMana(mana.Value);
                StartCoroutine(SuperAttack());
            }
            else
            {
                applySuperShootServerRpc();
            }
        }
    }

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
            boxCenter.x += gameObject.transform.localScale.x > 0 ? 6f : -6f;
            Vector2 boxSize = new Vector2(12f, 2f);
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
        if (GameManager.Instance.IsLocalMode()) //local mode
        {
            mana.Value++;
            manaBar.addMana();
        }
        else if (IsOwner) addManaServerRpc(); // in online mode only server modifies mana value, and only the owner can ask for it
    }

    private void MaxMana(GameObject g)
    {
        if (g != gameObject) return;
        if (GameManager.Instance.IsLocalMode()) // local mode
        {
            mana.Value = MAX_MANA;
            manaBar.FillMana();
        }
        else if (IsOwner) maxManaServerRpc(); // in online mode only server modifies mana value, and only the owner can ask for it
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = transform.position;
        boxCenter.x += gameObject.transform.localScale.x > 0 ? 6f : -6f;
        Vector2 boxSize = new Vector2(12f, 2f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    private void ShowLaserAnimation()//is called in the middle of super shoot animation
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) return;

        Vector3 direction = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

        for (int i = 0; i < laserCount; i++)
        {
            Vector3 spawnPos = laserStartPoint.position + direction * segmentSpacing * i;
            GameObject laserSegment = Instantiate(laserSegmentPrefab, spawnPos, Quaternion.identity);

            if (transform.localScale.x < 0)
            {
                Vector3 scale = laserSegment.transform.localScale;
                scale.x *= -1;
                laserSegment.transform.localScale = scale;
            }

            //spawn object if it is online mode
            if (!GameManager.Instance.IsLocalMode() && IsServer) laserSegment.GetComponent<NetworkObject>().Spawn();
        }
    }

    public int getMana()
    {
        return mana.Value;
    }

    public void setMana(int amount)//this method is only for save/load which is only for local mode
    {
        mana.Value = amount;
        manaBar.SetMana(mana.Value);
    }

    //ServerRpc
    [ServerRpc]
    private void applySuperShootServerRpc()
    {
        mana.Value = 0;
        UpdateManaBarClientRpc(mana.Value);
        StartCoroutine(SuperAttack());
    }

    [ServerRpc]
    private void addManaServerRpc()
    {
        mana.Value++;
        UpdateManaBarClientRpc(mana.Value);
    }

    [ServerRpc]
    private void maxManaServerRpc()
    {
        mana.Value = MAX_MANA;
        UpdateManaBarClientRpc(mana.Value);
    }

    //ClientRpc
    [ClientRpc]
    private void UpdateManaBarClientRpc(int value)
    {
        manaBar.SetMana(value);
    }
}