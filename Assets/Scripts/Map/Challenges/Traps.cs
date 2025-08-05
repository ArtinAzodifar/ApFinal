using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class Traps : NetworkBehaviour
{
    [SerializeField] private int damageAmount;
    private bool p1Trapped = false;
    private bool p1CoolDown = false;
    private bool p2Trapped = false;
    private bool p2CoolDown = false;
    private GameManager gameManager;
    private GameObject player1;
    private GameObject player2;

    private Coroutine p1TrapRoutine = null;
    private Coroutine p2TrapRoutine = null;

    public void Awake()
    {
        gameManager = GameManager.Instance;
        player1 = GameObject.FindWithTag("Player1");
        player2 = GameObject.FindWithTag("Player2");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return; //only server and local mode

        if (collision.gameObject.CompareTag("Player1") && !p1CoolDown && p1TrapRoutine == null)
        {
            p1Trapped = true;
            p1TrapRoutine = StartCoroutine(p1Trap());
        }

        if (collision.gameObject.CompareTag("Player2") && !p2CoolDown && p2TrapRoutine == null)
        {
            p2Trapped = true;
            p2TrapRoutine = StartCoroutine(p2Trap());
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameManager.IsLocalMode() && !IsServer) return; //only server and local mode

        if (collision.gameObject.CompareTag("Player1"))
        {
            p1Trapped = false;
            if (p1TrapRoutine != null)
            {
                StopCoroutine(p1TrapRoutine);
                p1TrapRoutine = null;
            }
            p1CoolDown = false;
        }

        if (collision.gameObject.CompareTag("Player2"))
        {
            p2Trapped = false;
            if (p2TrapRoutine != null)
            {
                StopCoroutine(p2TrapRoutine);
                p2TrapRoutine = null;
            }
            p2CoolDown = false;
        }
    }

    private IEnumerator p1Trap()
    {
        while (p1Trapped)
        {
            var Health = player1.GetComponent<PlayerHealth>();
            if (!Health.IsDying()) player1.GetComponent<Damagable>().Damage(damageAmount);
            p1CoolDown = true;
            yield return new WaitForSeconds(1f);
            p1CoolDown = false;
        }
    }
    private IEnumerator p2Trap()
    {
        while (p2Trapped)
        {
            var Health = player2.GetComponent<PlayerHealth>();
            if (!Health.IsDying()) player2.GetComponent<Damagable>().Damage(damageAmount);
            p2CoolDown = true;
            yield return new WaitForSeconds(1f);
            p2CoolDown = false;
        }
    }

}
