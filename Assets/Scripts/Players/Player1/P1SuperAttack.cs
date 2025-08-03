using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1SuperAttack : NetworkBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damageAmount;
    public NetworkVariable<int> mana = new NetworkVariable<int>(0);
    private const int MAX_MANA = 10;
    private Animator animator;
    [SerializeField] private ManaBar manaBar;
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
            resetManaBarClientRpc();
        }
    }

    private void OnEnable()
    {
        Player1Attack.P1Mana += ManaAdd;
        FullMana.ManaFill += MaxMana;
    }

    private void OnDisable()
    {
        Player1Attack.P1Mana -= ManaAdd;
        FullMana.ManaFill -= MaxMana;
    }

    //inputs:
    public void OnSuperAttack(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.IsLocalMode() && !IsOwner) return;
        if (context.performed && !gameObject.GetComponent<Player1Attack>().IsAttacking() &&
            !gameObject.gameObject.GetComponent<BaseControll>().IsInDamage() && mana.Value >= MAX_MANA && !gameObject.GetComponent<PlayerHealth>().IsDying())
        {
            if (gameManager.IsLocalMode())
            {
                mana.Value = 0;
                manaBar.SetMana(0);
                StartCoroutine(SuperAttack());
            }
            else if (IsOwner) applySuperShootServerRpc();
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
    private void resetManaBarClientRpc() { manaBar.SetMana(0); }

    private IEnumerator SuperAttack()
    {
        if (!GameManager.Instance.IsLocalMode() && !IsServer) yield break; //only local mode or the server(in online mode) can start this
        gameObject.GetComponent<Player1Attack>().setIsAttacking(true);
        animator.SetTrigger("SuperAttack");
        yield return new WaitForSeconds(0.5f);
        float pastTime = 0;
        float maxTime = 1.7f;
        while (pastTime < maxTime)
        {
            pastTime += Time.deltaTime;
            Vector2 boxCenter = transform.position;
            Vector2 boxSize = new Vector2(9.5f, 2f);
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
        yield return new WaitForSeconds(1.7f);
        gameObject.GetComponent<Player1Attack>().setIsAttacking(false);
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
        if (GameManager.Instance.IsLocalMode()) { mana.Value = MAX_MANA; } // local mode
        else if (IsOwner) maxManaServerRpc(); // in online mode only server modifies mana value, and only the owner can ask for it
        manaBar.FillMana();
    }
    [ServerRpc]
    private void maxManaServerRpc() { mana.Value = MAX_MANA; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = transform.position;
        Vector2 boxSize = new Vector2(9.5f, 2f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    public int getMana()
    {
        return mana.Value;
    }

    public void setMana(int amount)
    {
        mana.Value = amount;
        manaBar.SetMana(mana.Value);
    }
}